using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    private SphereCollider Range;
    public Material[] ButtonMaterial;//0 = Default, 1 = 可按, 2 = 按下
    public bool isPressed = false;
    public bool isInRange = false;
    void Start()
    {
        Range = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player" && !isPressed) {
            Debug.Log("Enter Range");
            this.transform.parent.GetComponent<MeshRenderer>().material = ButtonMaterial[1];
            isInRange = true;
            
            EventManager.ButtonPressedEvent += Press;
        }
        
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player" && !isPressed) {
            Debug.Log("Exit Range");
            this.transform.parent.GetComponent<MeshRenderer>().material = ButtonMaterial[0];
            isInRange = false;

            EventManager.ButtonPressedEvent -= Press;
        }
        
    }
    //在范围内按下按钮
    public void Press()
    {
        if (isInRange) 
        {
            PressButton();
        }
    }
    public void PressButton() 
    {
        if (isPressed) return;
        this.transform.parent.GetComponent<MeshRenderer>().material = ButtonMaterial[2];
        isPressed = true;
        EventManager.CallUpdateUIEvent(10);
    }
}
