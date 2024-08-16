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
        AudioSource.PlayClipAtPoint(breakSound,transform.position);
        //Destroy(this.gameObject);
        for(int i = 0; i < 3; i++)
        {
            GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            //coin.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-100, 100), Random.Range(100, 300)));
        }
    }
}
