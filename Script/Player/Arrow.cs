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
    [Tooltip("碰撞检测距离")]
    public float DectDistance = 1f;
    public Vector3 predictPos;
    public Vector3 predictForward;
    public Vector3 actualPos;
    public Vector3 actualForward;
    public LayerMask _mask;
    private float _velocity;
    void OnEnable() {
        Destroy(this.gameObject,10f);
        tracer = this.gameObject.GetComponentInChildren<TrailRenderer>();
    }
    public void InitPar(Vector3 _predictPos,Vector3 _predictForward,LayerMask mask,float velocity)
    {
        predictPos = _predictPos;
        predictForward = _predictForward;
        _mask = mask;
        _velocity = velocity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.forward = GetComponent<Rigidbody>().velocity.normalized;
        if(ColiderCheck())
        {
            JudgeError();
            if(!isReflect)
            {
                isReflect = true;
            }
            else
            {
                //取消rigidbody
                this.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }
    // 用射线检测碰撞
    bool ColiderCheck()
    {
        Ray ray = new Ray(this.transform.position, this.transform.forward);
        Debug.DrawRay(this.transform.position, this.transform.forward, Color.red, DectDistance);
        if (Physics.Raycast(ray, out RaycastHit hit, DectDistance,_mask))
        {
            RaycastHit[] hits = Physics.RaycastAll(ray, DectDistance,_mask);
            foreach (RaycastHit h in hits)
            {
                if (h.collider.gameObject.tag == "Button")
                {
                    h.collider.gameObject.GetComponentInChildren<ButtonScript>().PressButton();
                }
            }
            actualPos = hit.point;
            actualForward = Vector3.Reflect(this.transform.forward, hit.normal);
            return true;
        }
        return false;
    }

    void JudgeError()
    {
        //判断预测位置和实际位置的误差如果大于0.5f则认为是移动靶，重新设置反射位置和方向
        if (Vector3.Distance(predictPos, actualPos) < 0.5f)
        {
            Debug.Log("predictPos: " + predictPos + " actualPos: " + actualPos);
            Debug.Log("error: " + Vector3.Distance(predictPos, actualPos));
            this.transform.position = predictPos;
            this.transform.forward = predictForward;
            this.GetComponent<Rigidbody>().velocity = _velocity * this.transform.forward;
        }
        else
        {
            Debug.Log("predictPos: " + predictPos + " actualPos: " + actualPos);
            Debug.Log("error: " + Vector3.Distance(predictPos, actualPos));
            this.transform.position = actualPos;
            this.transform.forward = actualForward;
            this.GetComponent<Rigidbody>().velocity = _velocity * this.transform.forward;
        }
    }
}
