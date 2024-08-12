using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MapClass : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    /// <summary>
    /// 如何永远以屏幕中心为中心点进行放缩？例如地图app的放缩效果
    /// 平移在子物体进行，放缩在父物体进行
    /// </summary>

    // 控制自己的放缩
    private RectTransform rectTransform;        
    private float scrollWeelInput;              // 鼠标滚轮输入
    private float magnification = 1f;           // 放缩倍率
    private float Magnification
    {
        get { return magnification; }
        set
        {
            if (value < 1f)
            {
                value = 1f;
            }
            else if (value > 6f)
            {
                value = 6f;
            }
            magnification = value;
        }
    }
    [SerializeField]
    private float scaleSpeed = 250f;            // 放缩速度

    // 控制子物体的平移
    private RectTransform childRectTransform;
    private Vector3 onBeginDragRectPosition;
    private Vector3 onBeginDragOutPosition;
    private Vector3 onDragOutPosition;

    private void Awake()
    {
        // 成员初始化
        rectTransform = this.transform.GetComponent<RectTransform>();
        childRectTransform = this.transform.GetChild(0).GetComponent<RectTransform>();
    }

    private void Update()
    {
        scrollWeelInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWeelInput != 0f)
        {
            Magnification += scrollWeelInput * scaleSpeed * Time.deltaTime;
            rectTransform.localScale = Vector3.one * Magnification;
        }
    }

    /// <summary>
    /// 开始拖拽的时候记录图片的初始位置和根据鼠标位置的计算结果
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        onBeginDragRectPosition = childRectTransform.position;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            childRectTransform, eventData.position, eventData.enterEventCamera, out onBeginDragOutPosition);
    }

    /// <summary>
    /// 拖拽中根据鼠标位置的计算结果实时更新图片位置
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            childRectTransform, eventData.position, eventData.enterEventCamera, out onDragOutPosition);
        childRectTransform.position = onBeginDragRectPosition + onDragOutPosition - onBeginDragOutPosition;
    }


}

