using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBow : MonoBehaviour
{
    // Start is called before the first frame update
    float fireRate = 0.3f;
    public Transform firePoint;
    public GameObject arrowPrefab;
    public float MinForce = 20;
    public float currentForce; // 当前力度
    bool isCharging = false;
    public float maxForce = 100f; // 最大力度

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //从屏幕中心发射一条射线
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //射线碰撞到的地方
        RaycastHit hit;

        // 是否正在蓄力
        if (isCharging)
        {
            // 如果正在蓄力，增加力度值
            currentForce += Time.deltaTime * 20f; // 20是力度增加的速率，可以根据需要调整
            currentForce = Mathf.Min(currentForce, maxForce); // 限制最大力度
            Debug.Log("currentForce: " + currentForce);
        }


        if (Physics.Raycast(ray, out hit))
        {
            Vector3 hitPos = hit.point;
            transform.LookAt(hitPos);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
        if (fireRate > 0)
        {
            fireRate -= Time.deltaTime;
        }
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
        arrow.GetComponent<Rigidbody>().AddForce(firePoint.up * currentForce, ForceMode.Impulse);
        //Time.timeScale = 0.1f;
        Debug.Log(arrow.GetComponent<Rigidbody>().velocity);
        //Time.timeScale = 0.01f;
   
    }

    //用来调整射击力度，点击射击键后，按住不放，力度增加，松开射击键，射击
    public void OnCharge(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // 开始蓄力
            isCharging = true;
            currentForce = MinForce; // 初始化力度
        }
        else if (context.canceled && isCharging)
        {
            // 松开按键，执行攻击
            Fire();
            isCharging = false;
            currentForce = MinForce;
        }
    }
}
