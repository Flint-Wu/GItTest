using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    float threshold = 0.1f;//放置误触移动
    public float forwardspeed = 1.5f;
    public float sidespeed = 1.2f;
    public float backspeed = 1.5f;
    Vector2 currentspeed = Vector2.zero;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        transform.Translate(currentspeed.x * Time.deltaTime, 0, currentspeed.y * Time.deltaTime);
        //Debug.Log(currentspeed);
    }
    public void Move(InputAction.CallbackContext context)
    {
        Vector2 moveVec2 = context.ReadValue<Vector2>();
        if (moveVec2.x > threshold || moveVec2.x < -threshold)
        {
            currentspeed.x = moveVec2.x * sidespeed;
        }
        else if (moveVec2.y > threshold )
        {
            currentspeed.y = moveVec2.y * forwardspeed;
        }
        else if (moveVec2.y < -threshold)
        {
            currentspeed.y = moveVec2.y * backspeed;
        }
        else
        {
            currentspeed = Vector2.zero;
        }
    }

    //处理按钮交互
    public void Interact(InputAction.CallbackContext context)
    {
        Debug.Log("ButtonPressedEvent");
        EventManager.CallButtonPressedEvent();
    }
}
