using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParabolaDrawer : MonoBehaviour
{
    public Transform firePoint;
    public float[] Theta = new float[3];
    public float angle;
    //private List<GameObject> arrows = new List<GameObject>();
    public LineRenderer lineRenderer;
    public LineRenderer reflectLine;
    private Material linemat;//实现抛物线的动态效果
    private Material reflectmat;
    private int mainTex ;//缓存属性ID，提高效率https://blog.csdn.net/linxinfa/article/details/121507619
    private int reflectTex;
    public bool isReflect = false;
    public Transform reflectPoint;
    [Tooltip("用来储存反射特效")]
    public GameObject reflectEffect;
    public List<Vector3> TracePoints;//用于存储抛物线的点
    public List<Vector3> TraceForward;//用于存储抛物线的方向
    private LayerMask _mask;
    private void Start()
    {
        linemat = lineRenderer.material;
        mainTex = Shader.PropertyToID("_MainTex");
        reflectmat = reflectLine.material;
        reflectTex = Shader.PropertyToID("_MainTex");
        _mask = GetComponent<PlayerBow>().mask;

        reflectPoint = new GameObject("reflect").transform;
        reflectLine.gameObject.transform.parent = reflectPoint;
        reflectLine.gameObject.transform.position = Vector3.zero;
        reflectLine.gameObject.transform.rotation = Quaternion.identity;

        reflectEffect = Instantiate(reflectEffect, Vector3.zero, Quaternion.identity);

        
    }
    private void LateUpdate()
    {
        TracePoints.Clear();
        TraceForward.Clear();
        angle = GetComponent<PlayerBow>().currentAngle;
        //theta为forward方向分别与x轴,z轴,y轴的夹角(全局坐标系)
        GetCurve(firePoint,lineRenderer,true);
        if (isReflect)
        {
            GetCurve(reflectPoint, reflectLine, false);
        }
        //实现抛物线的动态效果
        linemat.SetTextureOffset(mainTex, new Vector2(-Time.time*2, 0));
        reflectmat.SetTextureOffset(reflectTex, new Vector2(-Time.time*2, 0));
        
    }
    //通过LineRender实时绘制抛物线,通过
    private void GetCurve(Transform origin,LineRenderer lineRenderer,bool DoReflect = false)
    {
        //DectPoints用于存储raycast检测的点，ParaPoints用于存储抛物线的点
        List<Vector3> DectPoints = new List<Vector3>();
        List<Vector3> ParaPoints = new List<Vector3>();
        if (!isReflect)
        {
            lineRenderer.positionCount = 0;
        }
        for (int i = 0; i < 600; i++)
        {
            float t = i * 0.02f;
            float z = 20 * t ;
            float y = 0.5f * Physics.gravity.y * t * t ;
            GameObject point = new GameObject("point");
            point.transform.parent = origin;
            point.transform.position = new Vector3(0, y, z);
            Destroy(point, 1f);

            ParaPoints.Add(point.transform.position);
            //将抛物线的点转换到世界坐标系
            DectPoints.Add(origin.TransformPoint(point.transform.position));
            TracePoints.Add(origin.TransformPoint(point.transform.position));
            
            
            //通过分段检测，绘制抛物线和检测碰撞的交点
            if(i>0 )
            {
                Vector3 direction = DectPoints[i] - DectPoints[i - 1];
                TraceForward.Add(direction);
                Ray segament = new Ray(DectPoints[i - 1], direction);
                float distance = Vector3.Distance(DectPoints[i - 1], DectPoints[i]);
                Debug.DrawLine(DectPoints[i - 1], DectPoints[i], Color.green, 0.1f);

                //如果第一次碰撞到物体，计算反射方向
                if (Physics.Raycast(segament, out RaycastHit hit, distance,_mask))
                {
                    if (DoReflect)
                    {

                    //计算入射角和反射角
                    Vector3 normal = hit.normal;
                    reflectPoint.position = hit.point;
                    reflectPoint.forward = Vector3.Reflect(direction, normal);
                    reflectEffect.transform.position = hit.point;
                    }
                    Debug.DrawRay(reflectPoint.position, reflectPoint.forward, Color.blue, 0.1f);
                    isReflect = true;
                    //将循环重置
                    break;
                }
                else 
                {
                    lineRenderer.positionCount = i + 1;
                    lineRenderer.SetPosition(i, ParaPoints[i]);
                }
            }
            //Ray segament = new Ray(new Vector3(0, y, z), new Vector3(0, y, z));
        }
    }
}

