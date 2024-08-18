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
    private float _currentTime;
    public List<ButtonScript> Buttons;
    public List<GameObject> lights;
    //建立按钮与状态的映射
    private Dictionary<ButtonScript, GameObject> _ButtonStatus = new Dictionary<ButtonScript, GameObject>();
    //public Material[] lightMaterial;//0 = Default, 1 = 开门
    public bool isOpen = false;
    public bool isInteract = false;//是否被触发过，time limited door用
    private Animator _animation;
    public float ClipTime;
    public AudioClip openSound;
    public AudioClip closeSound;
    public AudioClip timelimitedSound;

    void Start()
    {
        _animation = GetComponent<Animator>();

        if(type == DoorType.mutiple || type == DoorType.timelimited)
        {
            for (int i = 0; i < Buttons.Count; i++)
            {
                _ButtonStatus.Add(Buttons[i], lights[i]);
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
        if (type == DoorType.timelimited && isInteract)
        {
            if (_animation.GetCurrentAnimatorStateInfo(0).IsName("opendoor"))
            {
                //如果动画播放完毕，重置所有
                if (_animation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    _animation.SetBool("isTimelimited", true);
                    _animation.CrossFade("closedoor", 0.1f);
                }
            }
            
            if(_animation.GetCurrentAnimatorStateInfo(0).IsName("closedoor"))
            {
                _animation.speed = 1f / timeLimit;
                _currentTime -= Time.deltaTime;
                // ClipTime = 1 - _animation.GetCurrentAnimatorStateInfo(0).normalizedTime;
                ClipTime = _currentTime / timeLimit;
                if (_currentTime <= 0)
                {
                    ResetAll();
                    _currentTime = timeLimit;
                }
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
        _animation.speed = 0.2f;
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
        _ButtonStatus[button].GetComponent<Animator>().CrossFade("wireline_on", 0.1f);
        
    }
    public void LightOff(ButtonScript button)
    {
        // Renderer renderer = _ButtonStatus[button].GetComponent<Renderer>();
        // renderer.material = lightMaterial[0];
        _ButtonStatus[button].GetComponent<Animator>().CrossFade("wireline_off", 0.1f);
    }
    public void ResetAll()
    {
        foreach (var button in Buttons)
        {
            button.Rest();
        }
        isInteract = false;
        isOpen = false;
        _animation.speed = 1f;
        _animation.CrossFade("empty", 0f);
    }

    void PlayOpenDoorSound()
    {
        AudioSource.PlayClipAtPoint(openSound, this.transform.position);
    }

    void PlayCloseDoorSound()
    {
        AudioSource.PlayClipAtPoint(closeSound, this.transform.position);
    }

    void PlayTimeLimitedSound()
    {
        AudioSource.PlayClipAtPoint(timelimitedSound, this.transform.position);
    }
}
