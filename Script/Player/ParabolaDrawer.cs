using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaDrawer : MonoBehaviour
{
    public Transform firePoint;
    public float[] Theta = new float[3];
    public float velocity;
    //private List<GameObject> arrows = new List<GameObject>();
    public LineRenderer lineRenderer;
    private Material linemat;//实现抛物线的动态效果
    private int mainTex ;//缓存属性ID，提高效率https://blog.csdn.net/linxinfa/article/details/121507619
    private void Start()
    {
        linemat = lineRenderer.material;
        mainTex = Shader.PropertyToID("_MainTex");
        
    }
    private void Update()
    {
        velocity = GetComponent<PlayerBow>().currentForce;
        //theta为forward方向分别与x轴,z轴,y轴的夹角(全局坐标系)
        Theta[0] = Mathf.Deg2Rad * Mathf.Asin(firePoint.forward.y);
        Theta[1] = Mathf.Deg2Rad * Mathf.Asin(firePoint.forward.z);
        Theta[2] = Mathf.Deg2Rad * Mathf.Asin(firePoint.forward.x);
        GetCurve();
        //实现抛物线的动态效果
        linemat.SetTextureOffset(mainTex, new Vector2(-Time.time*2, 0));
        
        // if (Input.GetKeyDown(KeyCode.E))
        // {   
        //     // //物体的right方向与x轴呈theta角度
        //     // //GameObject Projectile = Instantiate<GameObject>(arrow, transform.position, Quaternion.identity);
        //     // Vector2 dir = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position).normalized;
        //     // Projectile.transform.right = dir;
        //     // Projectile.GetComponent<Rigidbody2D>().velocity = dir * force;
        //     // velocity = (dir * force).magnitude;
        //     // arrows.Add(Projectile);
        // }
        // //强制更新箭的方向沿着速度方向
        // foreach (GameObject arrow in arrows)
        // {
        //     Vector2 velocity = arrow.GetComponent<Rigidbody2D>().velocity;
        //     arrow.transform.right = velocity.normalized;
        // }
    }
    //通过LineRender实时绘制抛物线,通过
    private void GetCurve()
    {
        lineRenderer.positionCount = 20;//设置线段的数量
        for (int i = 0; i < 20; i++)
        {
            float t = i * 0.2f;
            float z = velocity * t ;
            float y = -0.5f * Physics.gravity.y * t * t ;
            lineRenderer.SetPosition(i, new Vector3(0, y, z));
            //Ray segament = new Ray(new Vector3(0, y, z), new Vector3(0, y, z));

        }
    }
}
