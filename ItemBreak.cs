using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBreak : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip breakSound;
    public GameObject coinPrefab;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Break()
    {
        
        //Destroy(this.gameObject);
        for(int i = 0; i < 3; i++)
        {
            GameObject coin = Instantiate(coinPrefab,transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0),Quaternion.identity);
            //coin.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-100, 100), Random.Range(100, 300)));
        }
        AudioSource.PlayClipAtPoint(breakSound,transform.position);
        Invoke("DisableCollider",1f);
    }
    void DisableCollider()
    {
        //取消所有子物体的碰撞器
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (var item in colliders)
        {
            item.enabled = false;
        }
    }
}
