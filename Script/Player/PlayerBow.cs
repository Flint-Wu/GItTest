using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class PlayerBow : MonoBehaviour
{
    // Start is called before the first frame update
    float fireRate = 0.3f;
    [Header("射击点")]
    public Transform firePoint;
    public Transform Bow;
    public GameObject arrowPrefab;
    public RaycastHit hit;
    public Vector3 hitPos;//记录当前射线碰撞点
    public LayerMask mask;
    public Vector3 raycastPoint;//记录蓄力时的射线碰撞点
    public Transform MouseTransform;//鼠标位置，用于指animation rig
    public Transform AutoLock;//自动瞄准的目标
    
    [Header("射击力度")]
    public float MinForce = 20;
    public float maxForce = 100f; // 最大力度
    public float currentForce = 20f; // 当前力度
    public LineRenderer ChargeLine;
    public bool isCharging = false;
    private StarterAssetsInputs inputs;
    private Animator _animator;
    public bool isPaused = false;

    void Start()
    {
        inputs = this.transform.root.GetComponent<StarterAssetsInputs>();
        //世界原点生成chargeLine
        ChargeLine = Instantiate(ChargeLine, Vector3.zero, Quaternion.identity);
        _animator = this.transform.root.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //从屏幕中心发射一条射线
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //射线碰撞到的地方
        hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit,100f,mask))
        {
            hitPos = hit.point;
            //自动瞄准逻辑

            if (hit.collider.gameObject.tag == "Button" && hit.collider.isTrigger&&
            !hit.collider.gameObject.GetComponent<ButtonScript>().isPressed)
            {
                AutoLock = hit.collider.gameObject.transform.parent;
                hitPos = AutoLock.position;    
                Debug.Log("自动瞄准");
                AutoLock.GetComponentInChildren<MeshRenderer>().material = AutoLock.GetComponentInChildren<ButtonScript>().ButtonMaterial[1];
                
                }
            else //如果不是按钮，取消自动瞄准
            {
                if (AutoLock != null&&!AutoLock.GetComponentInChildren<ButtonScript>().isPressed)
                {
                    AutoLock.GetComponentInChildren<MeshRenderer>().material = AutoLock.GetComponentInChildren<ButtonScript>().ButtonMaterial[0];
                    AutoLock = null;
                }
            }
            
        }


        if (fireRate > 0)
        {
            fireRate -= Time.deltaTime;
        }


        // 是否正在蓄力
        if (isCharging)
        {
            // 如果正在蓄力，增加力度值
            // 如果hitpos和raycastpoint的距离向量和firepoint的forward方向向量的点积大于0，说明向量同向，可以正确蓄力
            if (Vector3.Dot((raycastPoint - hitPos).normalized, firePoint.forward) < 0) return;
            currentForce = GetCurrentForce();
            //Debug.Log("currentForce: " + currentForce);
        }
        else
        {
            // 如果不在蓄力，更改player的朝向
            MouseTransform.position = hitPos;
        }

        //蓄力时不改变方向
        OnCharge();
        //计算方
    
    }
    public void Fire()
    {
        if (fireRate > 0)
        {
            return;
        }
        Debug.Log("Fire");
        fireRate = 0.3f;
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
        arrow.SetActive(true);
        arrow.GetComponent<Rigidbody>().AddForce(firePoint.forward * currentForce, ForceMode.Impulse);
        //Time.timeScale = 0.1f;
        Debug.Log(arrow.GetComponent<Rigidbody>().velocity);
        //Time.timeScale = 0.01f;
   
    }

    //用来调整射击力度，点击射击键后，按住不放，力度增加，松开射击键，射击
    public void OnCharge()
    {
        if(inputs.charge && !isCharging)
        {
            // 开始蓄力
            isCharging = true;
            currentForce = MinForce; // 初始化力度
            raycastPoint = hit.point;
            ChargeLine.gameObject.SetActive(true);
            //设置player的朝向
            //this.transform.root.right = -(hitPos - firePoint.position).normalized;

            _animator.CrossFade("Charge", 0f);
            _animator.SetBool("isCharge", true);
        }
        else if (!inputs.charge && isCharging)
        {
            // 松开按键，执行攻击
            Fire();
            isCharging = false;
            currentForce = MinForce;
            ChargeLine.gameObject.SetActive(false);
            _animator.SetBool("isCharge", false);
        }
    }

    public float GetCurrentForce()
    {
        //跟据鼠标位置计算射击力度
        this.currentForce = Mathf.Clamp(Vector3.Distance(raycastPoint, hitPos)*3, MinForce, maxForce);

        //绘制射击力度线
        ChargeLine.SetPosition(0, firePoint.position);
        ChargeLine.SetPosition(1, firePoint.position -firePoint.forward * currentForce*0.2f);
        ChargeLine.widthCurve = AnimationCurve.Linear(0, MinForce/100f, 1, currentForce/100f);
        
        //设置颜色渐变,使得力度越大，颜色越接近绿色
        Color endColor = Color.Lerp(Color.red, Color.green, currentForce / maxForce);
        Gradient ColorGradient = new Gradient();
        ColorGradient.SetKeys(  new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(endColor, 1.0f) }, 
                                new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) });
        ChargeLine.colorGradient = ColorGradient;

        return currentForce;
    }

}