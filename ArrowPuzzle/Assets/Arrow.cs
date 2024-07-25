using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 spawnPosition;
    Vector3 endPosition;
    float lifeTime;
    public TrailRenderer tracer;
    public Transform currentTransform;
    void OnEnable() {
        Destroy(this.gameObject,10f);
        tracer = this.gameObject.GetComponent<TrailRenderer>();
        currentTransform = this.gameObject.GetComponent<Transform>();

    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(this.gameObject.GetComponent<Rigidbody>().velocity); 
        tracer.transform.position = currentTransform.position;
    }
    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Button") {

            other.gameObject.GetComponentInChildren<ButtonScript>().PressButton();
            Debug.Log("Hit Button");
        }
        Destroy(this.gameObject);    
        
    }
}
