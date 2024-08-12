using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void Start()
    {
        
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // ���϶���ʼʱ����������ͼƬ��͸��������Ч��
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // �ƶ�ͼƬ
        rectTransform.anchoredPosition += eventData.delta / canvasGroup.transform.localScale.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // �����϶���ָ�ͼƬ״̬
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
}
