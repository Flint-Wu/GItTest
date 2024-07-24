using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 spawnPosition;
    Vector3 endPosition;
    float lifeTime;
    void OnEnable() {
     Destroy(this.gameObject,10f);
     Debug.Log("出生位置"+this.gameObject.transform.position);   
     Debug.Log("速度"+this.gameObject.GetComponent<Rigidbody>().velocity);
     spawnPosition = this.gameObject.transform.position;
     lifeTime = 0f;

    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(this.gameObject.GetComponent<Rigidbody>().velocity); 
        lifeTime += Time.deltaTime; 
    }
    void OnCollisionEnter(Collision other) {
        endPosition = this.gameObject.transform.position;

        Debug.Log("碰撞位置"+this.gameObject.transform.position);

        Debug.Log("下落高度"+(endPosition.y - spawnPosition.y));
        Debug.Log("飞行距离" + Mathf.Pow(Mathf.Pow(endPosition.x - spawnPosition.x, 2) + Mathf.Pow(endPosition.z - spawnPosition.z, 2), 0.5f));
        Debug.Log("生存时间"+lifeTime);
        Debug.Log("下落加速度"+(endPosition.y - spawnPosition.y) / (0.5f * lifeTime * lifeTime));

        Destroy(this.gameObject);    
        
    }
}
