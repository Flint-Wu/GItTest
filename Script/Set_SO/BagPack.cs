using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BagPack", menuName = "inventory/New BagPack")]
public class BagPack : ScriptableObject
{
    public List<Item> ItemList = new List<Item>();
}
