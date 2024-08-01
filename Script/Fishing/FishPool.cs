using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FishPool : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform FishHookTransform;
    public BoxCollider PoolRange;
    public List<FishAI> fishes;//用来储存鱼的列表
    void Start()
    {
        fishes = new List<FishAI>();
        fishes.AddRange(this.transform.parent.GetComponentsInChildren<FishAI>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //生成鱼的目标点
    public void StartFish(Transform fishHookTransform)
    {
        FishHookTransform = fishHookTransform;
        int index = Random.Range(0, fishes.Count);
        fishes[index].FindTarget();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FishHook")
        {
            StartFish(other.transform);
        }
    }
}
