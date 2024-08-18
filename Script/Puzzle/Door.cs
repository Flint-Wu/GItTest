using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;
using OpenCover.Framework.Model;

public class Door : MonoBehaviour
{
    public enum DoorType
    {
        single,
        mutiple,
        timelimited,
        rotate,
    }

    public class Line
    {
        public GameObject Wire;
        public DoorLight Light;
    }
    
    public DoorType type;
    // Start is called before the first frame update
    public float timeLimit = 10f;
    private float _currentTime;
    public List<ButtonScript> Buttons;
    public List<GameObject> lights;
    public List<DoorLight> doorLights;
    //建立按钮与状态的映射
    private Dictionary<ButtonScript, Line> _ButtonStatus = new Dictionary<ButtonScript,Line>();
    
    //public Material[] lightMaterial;//0 = Default, 1 = 开门
    public bool isOpen = false;
    public bool isInteract = false;//是否被触发过，time limited door用
    public bool isClosing = false;
    //private Animator _animation;
    [Header("门开关动画")]
    public GameObject door;
    public float updistance = 3f;
    public float upTime = 1f;

    //public float ClipTime;
    public AudioClip openSound;
    public AudioClip closeSound;
    public AudioClip timelimitedSound;

    void Start()
    {
        //_animation = GetComponent<Animator>();
        //door = this.transform.Find("door").gameObject;

        if(type == DoorType.mutiple || type == DoorType.timelimited)
        {
            for (int i = 0; i < Buttons.Count; i++)
            {
                Line line = new Line();
                line.Wire = lights[i];
                if (doorLights.Count > i)
                {
                    line.Light = doorLights[i];
                }
                _ButtonStatus.Add(Buttons[i], line);
            }
            _currentTime = timeLimit;
            //获取关闭门的state,名字叫closedoor
        }

        else if (type == DoorType.single)
        {
            if (Buttons.Count != 1 || lights.Count != 0)
            {
                Debug.LogError("DoorType is single but Buttons or lights count is not 1");
            }
        }

        foreach (var button in Buttons)
        {
           if(button.isPressed)
            {
                LightOn(button);
            }
        }

    }


    // Update is called once per frame
    void FixedUpdate() {
        
        //如果是时间限制的门，时间减少
        if (type == DoorType.timelimited && isOpen&&!isClosing)
        {
            CloseDoor();
            //this.GetComponentInChildren<TextMeshPro>().text = timeLimit.ToString("F2");
        }   
    }

    void OpenDoor()
    {
        //isOpen = true;
        isInteract = true;
        if (type != DoorType.rotate)
        {
            door.transform.DOMoveY(door.transform.position.y + updistance, upTime).OnComplete(() => isOpen = true);
            //this.GetComponentInChildren<TextMeshPro>().text = timeLimit.ToString("F2");
        }
        else
        {
            this.GetComponent<Animator>().CrossFade("opendoor", 0.1f);
        }

        PlayOpenDoorSound();
        //播放开门动画，tag为opendoor
        // _animation.CrossFade("opendoor", 0.1f);
        // _animation.speed = 0.2f;
    }

    void CloseDoor()
    {
        //isOpen = false;
        //结束时触发ResetAll
        door.transform.DOMoveY(door.transform.position.y - updistance, timeLimit).OnComplete(ResetAll);
        PlayCloseDoorSound();
        isClosing = true;

        //播放关门动画，tag为closedoor
        // _animation.CrossFade("closedoor", 0.1f);
        // _animation.speed = 1f;
    }
    
    public void CheckButton()
    {
        bool allPressed = true;
        foreach (var button in Buttons)
        {
            if (!button.isPressed)
            {
                allPressed = false;
                break;
            }
        }

        if (allPressed)
        {
            OpenDoor();
        }
    }
    public void LightOn(ButtonScript button)
    {
        Debug.Log("LightOn");
        _ButtonStatus[button].Wire.GetComponent<Animator>().CrossFade("wireline_on", 0.1f);
        if (_ButtonStatus[button].Light != null)
        {
            _ButtonStatus[button].Light.LightOn();
        }
        
        
    }
    public void LightOff(ButtonScript button)
    {
        // Renderer renderer = _ButtonStatus[button].GetComponent<Renderer>();
        // renderer.material = lightMaterial[0];
        _ButtonStatus[button].Wire.GetComponent<Animator>().CrossFade("wireline_off", 0.1f);
        if (_ButtonStatus[button].Light != null)
        {
            _ButtonStatus[button].Light.LightOff();
        }
    }
    public void ResetAll()
    {
        foreach (var button in Buttons)
        {
            button.Rest();
        }
        isInteract = false;
        isOpen = false;
        isClosing = false;
        // _animation.speed = 1f;
        // _animation.CrossFade("empty", 0f);
    }

    void PlayOpenDoorSound()
    {
        if (openSound == null)
        {
            return;
        }
        AudioSource.PlayClipAtPoint(openSound, this.transform.position);
    }

    void PlayCloseDoorSound()
    {
        if (closeSound == null)
        {
            return;
        }
        AudioSource.PlayClipAtPoint(closeSound, this.transform.position);
    }

    void PlayTimeLimitedSound()
    {
        if (timelimitedSound == null)
        {
            return;
        }
        AudioSource.PlayClipAtPoint(timelimitedSound, this.transform.position);
    }
}
