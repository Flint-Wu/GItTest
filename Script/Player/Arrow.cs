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
    public List<Vector3> TraceForward = new List<Vector3>();
    [Tooltip("碰撞检测距离")]
    public float DectDistance = 1f;
    void OnEnable() {
        Destroy(this.gameObject,10f);
        tracer = this.gameObject.GetComponentInChildren<TrailRenderer>();
        if (parabolaDrawer == null)
        {
            Debug.LogError("parabolaDrawer is null");
        }

        foreach (Vector3 point in parabolaDrawer.TracePoints)
        {
            TracePoints.Add(point);
        }
        foreach (Vector3 forward in parabolaDrawer.TraceForward)
        {
            TraceForward.Add(forward);
        }
        reflectPointFoward = parabolaDrawer.reflectPoint.forward;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        lifeTime += Time.deltaTime;
        int index = (int)(lifeTime * 50);
        ColiderCheck();
        if (index < TracePoints.Count - 1 && index < TraceForward.Count - 1)
        {
            this.transform.position = Vector3.Lerp(TracePoints[index], TracePoints[index + 1], index -lifeTime * 50);
            this.transform.forward = Vector3.Lerp(TraceForward[index], TraceForward[index + 1], index - lifeTime * 50);
        }
        
        //Debug.Log(this.gameObject.GetComponent<Rigidbody>().velocity); 
        //tracer.transform.position = this.transform.position;
    }
    // 用射线检测碰撞
    void ColiderCheck()
    {
        Ray ray = new Ray(this.transform.position, this.transform.forward);
        Debug.DrawRay(this.transform.position, this.transform.forward, Color.red, DectDistance);
        if (Physics.Raycast(ray, out RaycastHit hit, DectDistance))
        {
            if (hit.collider.gameObject.tag == "Button")
            {
                hit.collider.gameObject.GetComponentInChildren<ButtonScript>().PressButton();
                isReflect = true;
            }
            else
            {
                if (!isReflect)
                {
                    Debug.Log(hit.collider.gameObject.name);
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
