using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [Space(10)]
    [Tooltip("它表示角色在能够再次跳跃之前需要等待的时间")]
    public float JumpTimeout = 0.50f;
    [Tooltip("它表示角色在能够再次进入坠落状态之前需要等待的时间，用来下楼梯")]
    public float FallTimeout = 0.15f;
    [Header("角色落地检测")]
    [Tooltip("角色是否在地面上")]
    public bool Grounded = true;

    [Tooltip("在崎岖地面上很有用")]
    public float GroundedOffset = -0.14f;

    [Tooltip("地面检测的半径。应与 CharacterController 的半径匹配")]
    public float GroundedRadius = 0.28f;

    [Tooltip("地面layer")]
    public LayerMask GroundLayers;
    [Header("角色移动速度")]
    float threshold = 0.1f;//放置误触移动
    public float forwardspeed = 1.5f;
    public float sidespeed = 1.2f;
    public float backspeed = 1.5f;
    public float SprintSpeed = 5f;//冲刺速度
    Vector2 currentspeed = Vector2.zero;      
    // animation IDs
    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDMotionSpeed;  
    private bool _hasAnimator;
    private Animator _animator;
    // timeout deltatime
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;
    private void Start()
    {     
        _hasAnimator = TryGetComponent(out _animator);

        AssignAnimationIDs();

        // reset our timeouts on start
        _jumpTimeoutDelta = JumpTimeout;
        _fallTimeoutDelta = FallTimeout;
    }
    /// <summary>
    /// 分配动画ID
    /// </summary>
    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }
    //进行落地检测
    private void GroundedCheck()
    {
        // 在角色的脚下创建一个球形检测器
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);

        // update animator if using character
        if (_hasAnimator)
        {
            _animator.SetBool(_animIDGrounded, Grounded);
        }
    }


    // Update is called once per frame
    void Update()
    {
        _hasAnimator = TryGetComponent(out _animator);

        // JumpAndGravity();
        // GroundedCheck();
        // Move();
    }
    void FixedUpdate()
    {
        transform.Translate(currentspeed.x * Time.deltaTime, 0, currentspeed.y * Time.deltaTime);
        //Debug.Log(currentspeed);
    }
    public void Move(InputAction.CallbackContext context)
    {
        //蓄力时不不能移动
        if (GetComponentInChildren<PlayerBow>().isCharging) {
            currentspeed = Vector2.zero;
            return;
        }

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

    //处理按钮交互dw
    public void Interact(InputAction.CallbackContext context)
    {
        EventManager.CallButtonPressedEvent();
    }
}
