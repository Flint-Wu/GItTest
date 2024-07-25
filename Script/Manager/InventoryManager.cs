using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<GameObject> itemList = new List<GameObject>();

    private void OnEnable()
    {
        EventManager.ItemSave += OnItemSave;
    }

    private void OnDisable()
    {
        EventManager.ItemSave -= OnItemSave;
    }

    private void OnItemSave(GameObject gameObject)
    {
        itemList.Add(gameObject);
    }


}
