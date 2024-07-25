using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    private SphereCollider Range;
    public Material[] ButtonMaterial;//0 = Default, 1 = 可按, 2 = 按下
    void Start()
    {
        Range = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            Debug.Log("Enter Range");
            this.transform.root.GetComponent<MeshRenderer>().material = ButtonMaterial[1];
        }
        
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            Debug.Log("Exit Range");
            this.transform.root.GetComponent<MeshRenderer>().material = ButtonMaterial[0];
        }
        
    }
}
