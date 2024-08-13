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
    [Tooltip ("鼠标射线碰撞点的层，用来控制主角的瞄准点")]
    public LayerMask MouseMask;
    public Vector3 raycastPoint;//记录蓄力时的射线碰撞点
    public Transform AimTransform;//鼠标位置，用于指animation rig
    [Header("拉弓力度灵敏度")]
    public float angleSensitive = 2f;
    [Header("弓箭速度")]
    public float velocity = 10f;
    [Header("射击力度")]
    public float gravity = 15f;

    public float MinAngle = 0f;
    public float MaxAngle = 90f; // 最大力度
    public float currentAngle = 0f; // 当前力度
    public LineRenderer ChargeLine;
    public bool isCharging = false;
    private StarterAssetsInputs inputs;
    private Animator _animator;
    public bool isPaused = false;
    private ParabolaDrawer _pd;
    private Transform _playerTransform;

    void Start()
    {
        inputs = this.transform.root.GetComponent<StarterAssetsInputs>();
        //世界原点生成chargeLine
        ChargeLine = Instantiate(ChargeLine, Vector3.zero, Quaternion.identity);
        _animator = this.transform.root.GetComponent<Animator>();
        _pd = this.GetComponentInChildren<ParabolaDrawer>();
        _pd.InitPar(mask,velocity,gravity);
        _playerTransform = this.transform.root;
    }

    // Update is called once per frame
    void Update()
    {
        //从屏幕中心发射一条射线
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //射线碰撞到的地方
        hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit,500f,MouseMask))
        {
            hitPos = hit.point;
            Debug.DrawLine(ray.origin, hit.point, Color.red);
            //自动瞄准逻辑
            
        }

        if (fireRate > 0)
        {
            fireRate -= Time.deltaTime;
        }
        
        // 是否正在蓄力
        if (isCharging)
        {
            _pd.InitPar(mask,velocity,gravity);
            // 如果正在蓄力，增加力度值
            // 如果hitpos和raycastpoint的距离向量和firepoint的forward方向向量的点积大于0，说明向量同向，可以正确蓄力
            if (Vector3.Dot((raycastPoint - hitPos).normalized, firePoint.forward) < 0) return;
            currentAngle = GetCurrentAngle();
            //Debug.Log("currentForce: " + currentForce);
            float y = Vector3.Distance(firePoint.position, AimTransform.position)*Mathf.Tan(currentAngle*Mathf.Deg2Rad);
            AimTransform.position = new Vector3(2*transform.position.x-hitPos.x, transform.position.y + y,2*transform.position.z-hitPos.z);
            // AimTransform.position = new Vector3(hitPos.x, transform.position.y + y,hitPos.z);

            //旋转player的rotation方向
            Vector3 newRotation = new Vector3(0, Mathf.Atan2(hitPos.x - transform.position.x, hitPos.z - transform.position.z) * Mathf.Rad2Deg, 0);
            this.transform.root.rotation = Quaternion.Euler(newRotation+new Vector3(0,-90f,0));

            //FirePoint以player为，angle为当前角度绕forward旋转
            firePoint.localRotation = Quaternion.Euler(new Vector3(-currentAngle, -90f, 0));

        }
        else
        {
            // 如果没有蓄力，保持力度值为最小值
            AimTransform.position = new Vector3(hitPos.x, transform.position.y, hitPos.z);
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
        
        arrow.GetComponent<Arrow>().InitPar(_pd.endEffect.transform.position,_pd.endEffect.transform.forward,mask,velocity,gravity);
        arrow.SetActive(true);
        arrow.GetComponent<Rigidbody>().AddForce(firePoint.forward * velocity*arrow.GetComponent<Rigidbody>().mass, ForceMode.Impulse);
        
        Debug.Log(Vector3.up*(gravity-Physics.gravity.y));
        //设置箭矢的反射点
        //arrow.GetComponent<Arrow>().parabolaDrawer = this.GetComponentInChildren<ParabolaDrawer>();
        
        //Time.timeScale = 0.1f;
        //Time.timeScale = 0.01f;
   
    }

    //用来调整射击力度，点击射击键后，按住不放，力度增加，松开射击键，射击
    public void OnCharge()
    {
        if(inputs.charge && !isCharging)
        {
            // 开始蓄力
            isCharging = true;
            currentAngle = MinAngle; // 初始化力度
            raycastPoint = hit.point;
            ChargeLine.gameObject.SetActive(true);
            //设置player的朝向
            //this.transform.root.right = -(hitPos - firePoint.position).normalized;

            //显示蓄力线
            _pd.EnableLineRenderer(true);
            
            _animator.CrossFade("Charge", 0f);
            _animator.SetBool("isCharge", true);
        }
        else if (!inputs.charge && isCharging)
        {
            // 松开按键，执行攻击
            Fire();
            isCharging = false;
            currentAngle = MinAngle;
            ChargeLine.gameObject.SetActive(false);

            //隐藏指使线
            _pd.EnableLineRenderer(false);
            
            _animator.SetBool("isCharge", false);
        }
    }
    
    //更改射箭的角度
    public float GetCurrentAngle()
    {
        //跟据鼠标位置计算射击力度
        this.currentAngle = Mathf.Clamp(Vector3.Distance(this.transform.position, hitPos)*angleSensitive, MinAngle, MaxAngle);

        //绘制射击力度线
        ChargeLine.SetPosition(0, _playerTransform.position);
        Vector3 EndPos = _playerTransform.position - firePoint.forward * (currentAngle+10f)*0.2f;
        EndPos.y = _playerTransform.position.y;
        ChargeLine.SetPosition(1, EndPos);
        ChargeLine.widthCurve = AnimationCurve.Linear(0, 0.2f, 1, 0.2f+currentAngle/30f);
        
        //设置颜色渐变,使得力度越大，颜色越接近绿色
        Color endColor = Color.Lerp(Color.red, Color.green, currentAngle / MaxAngle);
        Gradient ColorGradient = new Gradient();
        ColorGradient.SetKeys(  new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(endColor, 1.0f) }, 
                                new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) });
        ChargeLine.colorGradient = ColorGradient;

        return currentAngle;
    }

}