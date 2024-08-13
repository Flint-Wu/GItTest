using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Start is called before the first frame update
    float lifeTime;
    public TrailRenderer tracer;
    //public bool isReflect = false;
    [Tooltip("碰撞检测距离")]
    public float DectDistance = 1f;
    public Vector3 predictPos;
    public Vector3 predictForward;
    public Vector3 actualPos;
    public Vector3 actualForward;
    public LayerMask _mask;
    private float _velocity;
    private Vector3 previousVelocity;
    private float _gravity;
    public GameObject hitEffect;
    void OnEnable() {
        Destroy(this.gameObject,10f);
        tracer = this.gameObject.GetComponentInChildren<TrailRenderer>();
    }
    public void InitPar(Vector3 _predictPos,Vector3 _predictForward,LayerMask mask,float velocity,float gravity)
    {
        predictPos = _predictPos;
        predictForward = _predictForward;
        _mask = mask;
        _velocity = velocity;
        _gravity = gravity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.forward = GetComponent<Rigidbody>().velocity.normalized;
        GetComponent<Rigidbody>().AddForce(Vector3.up*(_gravity-Physics.gravity.y),ForceMode.Acceleration);

        float Acceleration = (this.GetComponent<Rigidbody>().velocity-previousVelocity).magnitude*50f;
        Debug.Log($"Acceleration: {Acceleration}");
        previousVelocity = this.GetComponent<Rigidbody>().velocity;

        if(ColiderCheck())
        {
            JudgeError();
            // if(!isReflect)
            // {
            //     isReflect = true;
            // }
            // else
            // {
                //取消rigidbody
            GameObject effect = Instantiate(hitEffect,actualPos,Quaternion.identity);
            effect.transform.forward = actualForward;
            
            Destroy(effect,1f);
            Destroy(this.gameObject);

            //Destroy(this.gameObject);

            // }
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
