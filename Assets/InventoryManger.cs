using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManger : MonoBehaviour
{
    public GameObject Bow;
    // Start is called before the first frame update
    void Start()
    {
        Bow = GetComponentInChildren<PlayerBow>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void EnableBow(bool enable)
    {
        Bow.SetActive(enable);
    }
}
