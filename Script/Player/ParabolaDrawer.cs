using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class ParabolaDrawer : MonoBehaviour
{
    public Transform firePoint;
    public float angle;
    //private List<GameObject> arrows = new List<GameObject>();
    public LineRenderer lineRenderer;
    //public LineRenderer reflectLine;
    [Tooltip("用来储存地面投影线的,1是绘制最后到墙上的线,2是绘制到地面的线,这是为了解决落地点比较低的情况下投影线绘制不完全的问题")]
    public DecalProjector projector;
    public DecalProjector projector2;
    private Material linemat;//实现抛物线的动态效果
    //private Material reflectmat;
    private int mainTex ;//缓存属性ID，提高效率https://blog.csdn.net/linxinfa/article/details/121507619
    //private int reflectTex;
    //public bool isReflect = false;
    //public Transform reflectPoint;
    public GameObject endEffect;
    [Tooltip("用来储存反射特效")]
    //public GameObject reflectEffect;
    private LayerMask _mask;
    private float _velocity;
    private float _gravity;
    public GameObject UIcursor;
    private void Start()
    {
        linemat = lineRenderer.material;
        mainTex = Shader.PropertyToID("_MainTex");
        // reflectmat = reflectLine.material;
        // reflectTex = Shader.PropertyToID("_MainTex");
        

        // reflectPoint = new GameObject("reflect").transform;
        // reflectLine.gameObject.transform.parent = reflectPoint;
        // reflectLine.gameObject.transform.position = Vector3.zero;
        // reflectLine.gameObject.transform.rotation = Quaternion.identity;

        // reflectEffect = Instantiate(reflectEffect, Vector3.zero, Quaternion.identity);
        endEffect = Instantiate(endEffect, Vector3.zero, Quaternion.identity);
        EnableLineRenderer(false);

        
    }
    public void InitPar(LayerMask mask,float velocity,float gravity)
    {
        _velocity = velocity; 
        _mask =mask;
        _gravity = gravity;
    }

    private void FixedUpdate()
    {
        angle = GetComponent<PlayerBow>().currentAngle;
        //theta为forward方向分别与x轴,z轴,y轴的夹角(全局坐标系)
        GetCurve(firePoint,lineRenderer,false);
        // if (isReflect)
        // {
        //     GetCurve(reflectPoint, reflectLine, false);
        // }
        //实现抛物线的动态效果
        linemat.SetTextureOffset(mainTex, new Vector2(-Time.time*2, 0));
        // reflectmat.SetTextureOffset(reflectTex, new Vector2(-Time.time*2, 0));

        //实现地面投影线的效果
        GetProjector(firePoint.position, endEffect.transform.position);
        
    }
    public void EnableLineRenderer(bool isEnable)
    {
        
        lineRenderer.enabled = isEnable;
        //reflectLine.enabled = isEnable;
        projector.enabled = isEnable;
        projector2.enabled = isEnable;
        //reflectEffect.SetActive(isEnable);
        endEffect.SetActive(isEnable);
        UIcursor.SetActive(isEnable);
    }

    private void GetProjector(Vector3 StartPoint,Vector3 EndPoint)
    {
        float distance = Vector2.Distance(new Vector2(StartPoint.x, StartPoint.z), 
                                            new Vector2(EndPoint.x, EndPoint.z));
        //绘制到墙面的投影线
        projector.transform.position = EndPoint;
        projector.transform.forward =StartPoint- new Vector3(EndPoint.x, StartPoint.y, EndPoint.z);

        projector.size = new Vector3(0.1f, 20f, distance);
        projector.pivot = new Vector3(0, -10f, distance / 2);

        //绘制到地面的投影线,distance-0.5f是防止绘制到墙上
        projector2.transform.position = StartPoint;
        projector2.transform.forward = -projector.transform.forward;
        projector2.size = new Vector3(0.1f, 20f, distance-0.5f);
        projector2.pivot = new Vector3(0, 0f, distance / 2);

        //位置为鼠标在UI上的位置
        UIcursor.transform.position = Input.mousePosition; 
    }
    //通过LineRender实时绘制抛物线,通过
    private void GetCurve(Transform origin,LineRenderer lineRenderer,bool DoReflect = false)
    {
        //DectPoints用于存储raycast检测的点，ParaPoints用于存储抛物线的点

         List<Vector3> ParaPoints = new List<Vector3>();
    
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0,origin.position);
    
        // TraceForward.Add(origin.forward);
        for (int i = 0; i < 1000; i++)
        {
            
            float t = i * 0.02f;

            Vector3 velocity = origin.forward * _velocity;
            float x = velocity.x * t;
            float z = velocity.z * t;
            float y = velocity.y * t + 0.5f * _gravity * t * t;

            ParaPoints.Add(new Vector3(x, y, z) + origin.position);

            
            //通过分段检测，绘制抛物线和检测碰撞的交点
            if(i>0 )
            {
                Vector3 direction = ParaPoints[i] - ParaPoints[i - 1];
                Ray segament = new Ray(ParaPoints[i - 1], direction);
                float distance = Vector3.Distance(ParaPoints[i - 1], ParaPoints[i]);
                Debug.DrawLine(ParaPoints[i - 1], ParaPoints[i], Color.green, 0.1f);

                //计算反射点
                if (Physics.Raycast(segament, out RaycastHit hit, distance,_mask))
                {
                    Vector3 normal = hit.normal;
                    if (DoReflect)
                    {
                    //计算入射角和反射角
                    
                        // reflectPoint.position = hit.point;
                        // reflectPoint.forward = Vector3.Reflect(direction, normal);
                        // reflectEffect.transform.position = hit.point;
                    }
                    else
                    {
                        endEffect.transform.forward = normal;
                        endEffect.transform.position = hit.point + normal * 0.01f;
                        
                    }
                    // Debug.DrawRay(reflectPoint.position, reflectPoint.forward, Color.blue, 0.1f);
                    // isReflect = true;
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

