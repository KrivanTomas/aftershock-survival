using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;


[CreateAssetMenu(fileName = "KuroItem", menuName = "Items/ItemInventoryInfo", order = 1)]
public class KuroItem : ScriptableObject
{
    public int itemID;
    public Vector2 size;
    public Sprite sprite;
    public string itemName;
    public string itemDescription;
}
