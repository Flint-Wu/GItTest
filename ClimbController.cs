using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClimbController : MonoBehaviour
{
    Transform playerTransform;
    Animator animator;
    CharacterController characterController;

    #region 玩家姿态及相关动画参数阈值
    public enum PlayerPosture
    {
        noClimbing,
        Climbing
    };
    public enum ClimbPlayer
    {
        defaultPlayer,
        child,
        adult
    };
    //[HideInInspector]
    public PlayerPosture playerPosture = PlayerPosture.noClimbing;
    public ClimbPlayer climbPlayer = ClimbPlayer.defaultPlayer;
    //玩家运动状态

    #region 翻越相关
    PlayerSensor playerSensor;
    bool isClimbReady;

    int defaultClimbParameter = 0;
    int vaultParameter = 1;
    int lowClimbParameter = 2;
    int highClimbParameter = 3;
    int currentClimbparameter;

    public Vector3 leftHandPosition;
    Vector3 rightHandPosition;
    Vector3 rightFootPosition;
    private StarterAssetsInputs _input;
    #endregion




    // Start is called before the first frame update
    void Start()
    {
        playerTransform = transform;
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        playerSensor = GetComponent<PlayerSensor>();
        _input = GetComponent<StarterAssetsInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        SwitchPlayerStates();
        Jump();
        SetupAnimator();

        
        
    }


    #endregion


    /// <summary>
    /// 用于切换玩家的各种状态
    /// </summary>
    void SwitchPlayerStates()
    {
        switch (playerPosture)
        {
            //着陆，一个转出
            case PlayerPosture.noClimbing:
                if (isClimbReady)
                {
                    playerPosture = PlayerPosture.Climbing;
                }
                break;

            //攀爬，一个转出
            case PlayerPosture.Climbing:

                if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("攀爬") && !animator.IsInTransition(0))
                {
                    playerPosture = PlayerPosture.noClimbing;
                }
                break;
        }
        //现在这里统一把isClimbReady设置为false，后续添加一个专门的变量控制方法
        isClimbReady = false;
    }


    /// <summary>
    /// 跳跃方法
    /// </summary>
    void Jump()
    {
        if (playerPosture == PlayerPosture.noClimbing && _input.jump)
        {
            float velOffset = 0f;//速度修正，目前没有用到

            switch (playerSensor.ClimbDetection(playerTransform,  new Vector3(_input.move.x, 0.0f, _input.move.y), velOffset))
            {
                case PlayerSensor.NextPlayerMovement.jump:
                    break;

                case PlayerSensor.NextPlayerMovement.climbLow:

                    leftHandPosition = playerSensor.Ledge + Vector3.Cross(-playerSensor.ClimbHitNormal, Vector3.up) * 0.3f;   //左手的位置向左移0.3米
                    isClimbReady = true;
                    currentClimbparameter = lowClimbParameter;
                    _input.jump = false;
                    break;

                case PlayerSensor.NextPlayerMovement.climbHigh:
                    leftHandPosition = playerSensor.Ledge + Vector3.Cross(playerSensor.ClimbHitNormal, Vector3.up) * 0.3f;        //右手的位置处于ledge右边0.3米
                    rightHandPosition = playerSensor.Ledge + Vector3.Cross(playerSensor.ClimbHitNormal, Vector3.up) * 0.3f;        //右手的位置处于ledge右边0.3米
                    rightFootPosition = playerSensor.Ledge + Vector3.down * 1.2f;   //脚的位置在顶端以下1.2米
                    isClimbReady = true;
                    currentClimbparameter = highClimbParameter;
                    _input.jump = false;
                    break;

            }
        }
    }

    /// <summary>
    /// 设置动画状态机的参数
    /// </summary>
    void SetupAnimator()
    {
        if (playerPosture == PlayerPosture.noClimbing)
        {
            characterController.enabled = true;
            animator.applyRootMotion = false;
            return;
            
        } 
        else if (playerPosture == PlayerPosture.Climbing)
        {
            characterController.enabled = false;
            animator.applyRootMotion = true;

            animator.SetInteger("攀爬方式", currentClimbparameter);
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        

            playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, Quaternion.LookRotation(-playerSensor.ClimbHitNormal), 0.5f);
            switch (climbPlayer)
            {
                case ClimbPlayer.defaultPlayer:   
                    if (info.IsName("ClimbLow"))
                    {
                        currentClimbparameter = defaultClimbParameter;
                        animator.MatchTarget(leftHandPosition, Quaternion.identity, AvatarTarget.LeftHand, new MatchTargetWeightMask(Vector3.one, 0f), 0f, 0.1f);
                        animator.MatchTarget(leftHandPosition + Vector3.up * 0.18f, Quaternion.identity, AvatarTarget.LeftHand, new MatchTargetWeightMask(Vector3.up, 0f), 0.1f, 0.3f);
                    }
                    else if (info.IsName("ClimbHigh"))
                    {
                        currentClimbparameter = defaultClimbParameter;
                        animator.MatchTarget(rightFootPosition, Quaternion.identity, AvatarTarget.RightFoot, new MatchTargetWeightMask(Vector3.one, 0f), 0f, 0.13f);
                        animator.MatchTarget(rightHandPosition, Quaternion.identity, AvatarTarget.RightHand, new MatchTargetWeightMask(Vector3.one, 0f), 0.2f, 0.32f);
                    }
                    break;
                
                case ClimbPlayer.child:
                    if (info.IsName("ClimbLow"))
                    {
                        currentClimbparameter = defaultClimbParameter;
                        animator.MatchTarget(leftHandPosition, Quaternion.identity, AvatarTarget.LeftHand, new MatchTargetWeightMask(Vector3.one, 0f), 0f, 0.6f);
                        //animator.MatchTarget(leftHandPosition + Vector3.up * 0.18f, Quaternion.identity, AvatarTarget.LeftHand, new MatchTargetWeightMask(Vector3.up, 0f), 0.6f, 0.9f);
                    }
                    else if (info.IsName("ClimbHigh"))
                    {
                        currentClimbparameter = defaultClimbParameter;
                        animator.MatchTarget(leftHandPosition, Quaternion.identity, AvatarTarget.LeftHand, new MatchTargetWeightMask(Vector3.one, 0f), 0f, 0.6f);
                        //animator.MatchTarget(leftHandPosition + Vector3.up * 0.18f, Quaternion.identity, AvatarTarget.LeftHand, new MatchTargetWeightMask(Vector3.up, 0f), 0.6f, 0.76f);
                    }
                    break;
                // else if (info.IsName("翻越"))
                // {
                //     currentClimbparameter = defaultClimbParameter;
                //     animator.MatchTarget(rightHandPosition, Quaternion.identity, AvatarTarget.RightHand, new MatchTargetWeightMask(Vector3.one, 0f), 0.1f, 0.2f);
                //     animator.MatchTarget(rightHandPosition + Vector3.up * 0.1f, Quaternion.identity, AvatarTarget.RightHand, new MatchTargetWeightMask(Vector3.one, 0f), 0.35f, 0.45f);
                // }
            }
        }

    }


    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(rightHandPosition, 0.1f);
    }

} 