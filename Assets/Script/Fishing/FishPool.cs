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
    public GameObject FishPrefab;
    public int FishCount;
    public FishingRod fishingRod;
    private GameObject _player;
    void Start()
    {
        fishes = new List<FishAI>();
        fishes.AddRange(this.transform.GetComponentsInChildren<FishAI>());
        fishingRod.gameObject.SetActive(false);
        _player = GameObject.FindGameObjectWithTag("Player");
        
        for (int i = 0; i < FishCount; i++)
        {
            GameObject fish = Instantiate(FishPrefab, new Vector3(Random.Range(PoolRange.bounds.min.x, PoolRange.bounds.max.x), PoolRange.bounds.min.y, Random.Range(PoolRange.bounds.min.z, PoolRange.bounds.max.z)), Quaternion.identity);
            fish.transform.parent = this.transform;
            fishes.Add(fish.GetComponent<FishAI>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //生成鱼的目标点
    public void StartFish()
    {
        FishHookTransform = fishingRod.gameObject.transform.Find("鱼钩");
        int index = Random.Range(0, fishes.Count);
        fishes[index].FindTarget();
        _player.GetComponent<Animator>().CrossFade("fishingstart", 0.1f);
        _player.GetComponent<Animator>().SetBool("isFishing", true);
        _player.GetComponent<InventoryManger>().EnableBow(false);
        fishingRod.gameObject.SetActive(true);
        EventManager.InteractEvent -= StartFish;
        EventManager.InteractEvent += ExitFish;
    }

    public void ExitFish()
    {
        _player.GetComponent<Animator>().SetBool("isFishing", false);
        _player.GetComponent<InventoryManger>().EnableBow(true);;
        fishingRod.gameObject.SetActive(false);
    }
    void OnTriggerEnter(Collider other)
    {
        EventManager.InteractEvent += StartFish;
    }

    void OnTriggerExit(Collider other)
    {
        EventManager.InteractEvent -= StartFish;
    }

}
