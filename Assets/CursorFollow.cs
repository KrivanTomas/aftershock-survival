using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorFollow : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float itemSize;
    [SerializeField] private RectTransform cursorItemTran;
    [SerializeField] private RawImage cursorItemImage;
    public InventoryFunction.ItemInfo Obsahuje { get; private set;}
    void Start()
    {
        Obsahuje = new InventoryFunction.ItemInfo(Item.CreateInstance<Item>(),0,0);
    }

    // Update is called once per frame
    void Update()
    {
        cursorItemTran.position = Input.mousePosition;
        cursorItemTran.sizeDelta = new Vector2(itemSize,itemSize);
    }
    public void NastavItem(InventoryFunction.ItemInfo item)
    {
        Obsahuje = item;
        cursorItemImage.enabled = true;
        cursorItemImage.texture = item.item.Icon;
    }
    public void Vypni()
    {
        cursorItemImage.enabled = false;
    }
}
