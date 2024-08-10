using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_block : MonoBehaviour
{
    private RectTransform _RectTransform;
    public RectTransform Target_RectTransform;
    private Vector3 StartSpeed = Vector3.zero;
    public float ChangeSpeed;


    // Start is called before the first frame update
    void Start()
    {
        _RectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        _RectTransform.localPosition = Vector3.SmoothDamp(_RectTransform.localPosition, Target_RectTransform.localPosition,ref StartSpeed, ChangeSpeed);
    }
}
