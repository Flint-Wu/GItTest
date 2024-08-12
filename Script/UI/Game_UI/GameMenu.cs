using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public RectTransform rectTransform;
    public GameObject gameMenu;
    public Menu_block menu_Block;



    void Start()
    {
        //gameMenu.gameObject.SetActive(false);
    }

    private void Awake()
    {
        rectTransform = FindObjectOfType<Menu_block>().transform.parent.GetComponentsInChildren<RectTransform>()[2];


        menu_Block = GetComponentInChildren<Menu_block>();
        gameMenu = FindObjectOfType<Menu_block>().transform.parent.gameObject;
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
