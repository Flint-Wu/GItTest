using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public RectTransform rectTransform;
    public GameObject gameMenu;
    private Menu_block menu_Block;

    // Start is called before the first frame update
    void Start()
    {
        gameMenu.gameObject.SetActive(false);
    }

    private void Awake()
    {
        menu_Block = GetComponentInChildren<Menu_block>();
        ChangeRect(rectTransform);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeRect(RectTransform rect)
    {
        menu_Block.Target_RectTransform = rect;
        int childCount = rectTransform.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = rectTransform.transform.GetChild(i);
            child.gameObject.SetActive(false);
        }
        
        rectTransform = rect;

        childCount = rectTransform.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = rectTransform.transform.GetChild(i);
            child.gameObject.SetActive(true);
        }
    }
}
