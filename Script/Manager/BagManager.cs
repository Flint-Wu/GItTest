using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagManager : MonoBehaviour
{

    static BagManager instance;

    public BagPack BagPack;

    public List<Image> slots = new List<Image>();
    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    private void OnEnable()
    {
        RefreshList();
    }


    public static void RefreshList()
    {
        for (int i = 0; i < instance.BagPack.ItemList.Count; i++)
        {
            if (instance.BagPack.ItemList.Count == 0)
            {
                break;
            }
            instance.slots[i] = null;
        }

        for (int i = 0; i < instance.BagPack.ItemList.Count; i++)
        {
            instance.slots[i].sprite = instance.BagPack.ItemList[i].itemImg;
        }
    }
}
