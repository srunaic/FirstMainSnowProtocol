using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSeonghyo;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
public class Item : ScriptableObject
{
    public int id;
    public string ItemName;
    public int value;
    public Sprite icon;
    public int count;
}
