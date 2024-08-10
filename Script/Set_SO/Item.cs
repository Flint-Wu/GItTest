using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "inventory/New Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public int holdCount;

    public Sprite itemImg;

    [TextArea]
    public string itemInfo;

    public bool used;

}
