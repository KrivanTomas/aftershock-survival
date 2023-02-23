using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemSlotScript : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] public Vector2 Size_1x1 {get; private set;}
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform itemSlot;
    private Item item;
    public Item Item{
        get
        {
            return item;
        }
        set
        {
            item = value;
            itemSlot.sizeDelta = new Vector2(Size_1x1.x * item.SizeX, Size_1x1.y * item.SizeY);
        }        
    }
    private void Start()
    {
        Size_1x1 = new Vector2(200,200);
    }
    private Vector2 previousPosition;
    public void SetPreviousPosition()
    {
        itemSlot.anchoredPosition = previousPosition;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        previousPosition = itemSlot.anchoredPosition;
        canvasGroup.blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        itemSlot.anchoredPosition += eventData.delta;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        if(eventData.pointerCurrentRaycast.gameObject == null)
            itemSlot.anchoredPosition = previousPosition;
    }
    public void OnDrop(PointerEventData eventData)
    {
        eventData.pointerDrag.gameObject.GetComponent<ItemSlotScript>().SetPreviousPosition();
    }
    


}
