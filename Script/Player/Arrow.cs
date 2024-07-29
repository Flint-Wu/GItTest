using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Start is called before the first frame update
    public ParabolaDrawer parabolaDrawer;
    float lifeTime;
    public TrailRenderer tracer;
    public bool isReflect = false;
    public Vector3 reflectPointFoward;
    public List<Vector3> TracePoints = new List<Vector3>();
    [Tooltip("碰撞检测距离")]
    public float DectDistance = 1f;
    void OnEnable() {
        Destroy(this.gameObject,10f);
        tracer = this.gameObject.GetComponentInChildren<TrailRenderer>();

        foreach (Vector3 point in parabolaDrawer.TracePoints)
        {
            TracePoints.Add(point);
        }
        reflectPointFoward = parabolaDrawer.reflectPoint.forward;
        Time.timeScale = 0.1f;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        lifeTime += Time.deltaTime;
        int index = (int)(lifeTime * 50);
        if (index >= TracePoints.Count - 1)
        {
            return;
        }
        this.transform.position = Vector3.Lerp(TracePoints[index], TracePoints[index + 1], index -lifeTime * 50);
        ColiderCheck();
        //Debug.Log(this.gameObject.GetComponent<Rigidbody>().velocity); 
        //tracer.transform.position = this.transform.position;
    }

    void ColiderCheck()
    {
        Ray ray = new Ray(this.transform.position, this.transform.forward);
        Debug.DrawRay(this.transform.position, this.transform.forward, Color.red, DectDistance);
        if (Physics.Raycast(ray, out RaycastHit hit, DectDistance))
        {
            if (hit.collider.gameObject.tag == "Button")
            {
                hit.collider.gameObject.GetComponentInChildren<ButtonScript>().PressButton();
            }
            else
            {
                if (!isReflect)
                {
                    this.transform.forward = reflectPointFoward;
                    isReflect = true;
                }
                //只能反弹一次
                else
                {
                    Destroy(this.gameObject); 
                }
            }
        } 
    }
}
