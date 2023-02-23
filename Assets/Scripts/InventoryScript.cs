
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;


public class InventoryScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDropHandler
{
    // Start is called before the first frame update
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GraphicRaycaster graphicRaycaster;
    [SerializeField] private GameObject itemSlotPref;
    [SerializeField] private RectTransform inventory;
    void Start()
    {
        Invoke("AddToInventory",3);
        
    }
    void Update()
    {
        //Debug.Log(new Vector3(inventory.anchorMin.x, inventory.anchorMin.y));
        //Debug.DrawLine(Vector3.zero, inventory.position, Color.red, 10);
    }
    public bool AddToInventory()
    {
        List<RaycastResult> result = new List<RaycastResult>();
        RectTransform itemSlot = (Instantiate(itemSlotPref,inventory).transform as RectTransform);
        PointerEventData pointerEventData = new PointerEventData(eventSystem);
        Vector3[] corners = new Vector3[4];
        inventory.GetWorldCorners(corners);
        //Debug.DrawLine(Vector3.zero, corners[1], Color.black, 10);
        for(float i = itemSlot.sizeDelta.x/2;i < inventory.sizeDelta.x;i += itemSlot.sizeDelta.x)
        {
            for(float j = itemSlot.sizeDelta.y/2;j<inventory.sizeDelta.y;j += itemSlot.sizeDelta.y)
            {
                
                pointerEventData.position = corners[1] + new Vector3(i,-j);
                Debug.DrawLine(corners[1], pointerEventData.position, Color.black, 10);
                
                
                
            }
        }
        pointerEventData.position = inventory.position;
        graphicRaycaster.Raycast(pointerEventData,result);
        //foreach(RaycastResult pointer in result)
        Debug.Log(result.Count);
        

        return true;
    }
    
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        
    }
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        
    }
    public void OnDrop(PointerEventData eventData)
    {
        Item item = eventData.pointerDrag.gameObject.GetComponent<ItemSlotScript>().Item;
        RectTransform itemSlot = (eventData.pointerDrag.gameObject.transform as RectTransform);
        ItemSlotScript itemSlotScript = eventData.pointerDrag.gameObject.GetComponent<ItemSlotScript>();
        Vector2 cursorOffset = (Vector2)eventData.position - (Vector2)itemSlot.position;
        Vector2 targetPosition = itemSlot.anchoredPosition + cursorOffset;
        Vector2 offsetCenter = new Vector2(targetPosition.x % (itemSlotScript.Size_1x1.x) - itemSlot.sizeDelta.x / 2,targetPosition.y % (itemSlotScript.Size_1x1.x) + itemSlot.sizeDelta.y / 2);
        itemSlot.anchoredPosition -= offsetCenter - cursorOffset;
    }
}
