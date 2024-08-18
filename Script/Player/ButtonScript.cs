using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    private SphereCollider Range;
    //public Material[] ButtonMaterial;//0 = Default, 1 = 可按, 2 = 按下
    public bool isPressed = false;
    public bool isInRange = false;
    public Door door;
    bool _hasAnimator = false;
    public AudioClip[] PressSounds;
    public ParticleSystem Shine;
    void Start()
    {
        Range = GetComponent<SphereCollider>();
        door = this.transform.root.GetComponent<Door>();
        if(this.transform.parent.GetComponent<Animator>() != null)
        {
            _hasAnimator = true;
        }
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
            //this.transform.parent.GetComponent<MeshRenderer>().material = ButtonMaterial[1];
            isInRange = true;
            
            //EventManager.InteractEvent += Press;
        }
        
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player" && !isPressed) {
            Debug.Log("Exit Range");
            //this.transform.parent.GetComponent<MeshRenderer>().material = ButtonMaterial[0];
            isInRange = false;

            //EventManager.InteractEvent -= Press;
        }
        
    }
    public void Rest()
    {
        if (isPressed) 
        {
            //this.transform.parent.GetComponent<MeshRenderer>().material = ButtonMaterial[0];
            if(_hasAnimator)
            {
                this.transform.parent.GetComponent<Animator>().CrossFade("closedoor", 0.1f);
            }
            isPressed = false;
            if(door.type == Door.DoorType.mutiple || door.type == Door.DoorType.timelimited)
            {
                door.LightOff(this);
            }
        }
    }
    //在范围内按下按钮
    public void Press()
    {
        if (isInRange) 
        {
            GameObject.FindWithTag("Player").GetComponent<Animator>().CrossFade("button push", 0.1f);
            PressButton();
        }
    }
    public void PressButton() 
    {
        if (isPressed) return;
        if(_hasAnimator)
        {
            this.transform.parent.GetComponent<Animator>().CrossFade("opendoor", 0.1f);
        }
        isPressed = true;
        EventManager.CallUpdateUIEvent(10);
        door.CheckButton();
        if(Shine != null)
            Shine.Stop();
        
        
        if(PressSounds.Length > 0)
        {
            AudioSource.PlayClipAtPoint(PressSounds[UnityEngine.Random.Range(0, PressSounds.Length)], this.transform.position);
        }

        if(door.type == Door.DoorType.mutiple || door.type == Door.DoorType.timelimited)
        {
            door.LightOn(this);
            if(door.type == Door.DoorType.timelimited)
            {
                door.isInteract = true;
            }
        }
    }
    
}
