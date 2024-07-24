using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    // Start is called before the first frame update
    float fireRate = 0.3f;
    public Transform firePoint;
    public GameObject arrowPrefab;

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
    public void Fire(InputAction.CallbackContext context)
    {
        if (fireRate > 0)
        {
            return;
        }
        Debug.Log("Fire");
        fireRate = 0.3f;
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
        arrow.GetComponent<Rigidbody>().velocity =  arrow.transform.forward * 30;
    }
}
