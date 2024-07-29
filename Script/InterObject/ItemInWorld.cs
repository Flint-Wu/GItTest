using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInWorld : MonoBehaviour
{
    public BagPack BagPack;
    public Item thisItem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Add2List();
            Destroy(gameObject);
        }
    }

    public void Add2List()
    {
        if (!BagPack.ItemList.Contains(thisItem))
        {
            for (int i = 0; i < BagPack.ItemList.Count; i++)
            {
                if (BagPack.ItemList[i] == null)
                {
                    BagPack.ItemList[i] = thisItem;
                }
            }
            thisItem.holdCount++;
        }
        else
        {
            thisItem.holdCount++;
        }
        BagManager.RefreshList();
    }
}
