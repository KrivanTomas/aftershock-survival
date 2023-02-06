using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "Assets/Item")] 
public class ItemObject : ScriptableObject
{
    public string Id;
    public string Name;
    public string Description;
    public Sprite Icon;
}
