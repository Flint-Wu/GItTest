
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBreak : MonoBehaviour
{
    // Start is called before the first frame update
    public enum Enum
    {
        Tree,
        Jar,
    }
    public Enum itemType;
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
        switch (itemType)
        {
            case Enum.Tree:
                BreakTree();

                break;
            case Enum.Jar:
                BreakJar();
                break;
        }
    }

    void BreakTree()
    {
        //设置所有子物体的coliider为false
        foreach (Transform child in transform)
        {
            StartCoroutine(DisableCollider(child));
        }
        AudioSource.PlayClipAtPoint(breakSound,transform.position);
    }

    void BreakJar()
    {
                //Destroy(this.gameObject);
        for(int i = 0; i < 3; i++)
        {
            GameObject coin = Instantiate(coinPrefab,transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0),Quaternion.identity);
            //coin.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-100, 100), Random.Range(100, 300)));
        }
        
        Invoke("DisableCollider",2f);
    }
    // void DisableCollider()
    // {

    // }
    IEnumerator DisableCollider(Transform child)
    {
        yield return new WaitForSeconds(1f);

        child.GetComponent<MeshCollider>().enabled = false;
        child.GetComponent<Rigidbody>().isKinematic = true;
    }
}
