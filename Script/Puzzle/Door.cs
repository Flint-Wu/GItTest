using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    public enum DoorType
    {
        single,
        mutiple,
        timelimited,
    }
    public DoorType type;
    // Start is called before the first frame update
    public float timeLimit = 10f;
    public List<ButtonScript> Buttons;
    public List<GameObject> lights;
    //建立按钮与状态的映射
    private Dictionary<ButtonScript, GameObject> _ButtonStatus = new Dictionary<ButtonScript, GameObject>();
    public Material[] lightMaterial;//0 = Default, 1 = 开门
    public bool isOpen = false;
    public bool isInteract = false;//是否被触发过，time limited door用
    private Animator _animation;

    void Start()
    {
        _animation = GetComponent<Animator>();

        if(type == DoorType.mutiple || type == DoorType.timelimited)
        {
            for (int i = 0; i < Buttons.Count; i++)
            {
                _ButtonStatus.Add(Buttons[i], lights[i]);
            }
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
    void Update()
    {
        if(isOpen)
        {
            return;
        }
        //如果是时间限制的门，时间减少
        if (type == DoorType.timelimited && isInteract)
        {
            timeLimit -= Time.deltaTime;
            if (timeLimit <= 0)
            {
                ResetAll();
                timeLimit = 10f;
            }
            //this.GetComponentInChildren<TextMeshPro>().text = timeLimit.ToString("F2");
        }   
    }

    void OpenDoor()
    {
        isOpen = true;
        isInteract = true;
        //播放开门动画，tag为opendoor
        _animation.CrossFade("opendoor", 0.1f);
    }
    void CloseDoor()
    {
        isOpen = false;
        isInteract = false;
        //更改closedoor的speed为 0.5/Timelimit
        _animation.speed = 1/ timeLimit;
        _animation.CrossFade("closedoor", 0.1f);
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
        Renderer renderer = _ButtonStatus[button].GetComponent<Renderer>();
        renderer.material = lightMaterial[1];
    }
    public void LightOff(ButtonScript button)
    {
        Renderer renderer = _ButtonStatus[button].GetComponent<Renderer>();
        renderer.material = lightMaterial[0];
    }
    public void ResetAll()
    {
        foreach (var button in Buttons)
        {
            button.Rest();
        }
        isInteract = false;
    }
}
