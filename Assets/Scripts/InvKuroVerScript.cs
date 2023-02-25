using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InvKuroVerScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    [Header("Settings")]
    [SerializeField] private Vector2 gridSize = new Vector2(10f,10f);    
    [SerializeField] private GameObject itemPrefab;    
    [SerializeField] private KuroItem testItemData1;    
    [SerializeField] private KuroItem testItemData2;    

    private GameObject dragGameObject;
    private KuroItemHandle dragItemHandle;
    private RectTransform inventoryRectTransform;

    private Rect inventoryBounds;


    private List<KuroItemHandle> items = new List<KuroItemHandle>();

    void Start(){
        inventoryRectTransform = gameObject.GetComponent<RectTransform>();
        inventoryBounds = inventoryRectTransform.rect;

        for(int i = 0; i < 10; i++){
        items.Add(AddItemTest(testItemData1));
        }
        items.Add(AddItemTest(testItemData2));
    }

    void Update()
    {
        // Move dragged GameObject while snapping to grid
        Vector2 localPoint;
        
        if(dragGameObject && RectTransformUtility.ScreenPointToLocalPointInRectangle(inventoryRectTransform, Input.mousePosition, Camera.current, out localPoint)){
            // Rotate the item
            if(Input.GetKeyDown(KeyCode.R)){
                dragItemHandle.Rotate();
            }
            float tileSize = inventoryBounds.size.x / gridSize.x;
            // Shift the grid for odd axis
            Vector3 oddOffset = 
                Vector3.right * (dragItemHandle.finalSize.x % 2 == 0 ? tileSize * 0.5f : 0f) +
                Vector3.up * (dragItemHandle.finalSize.y % 2 == 0 ? tileSize * 0.5f : 0f);

            dragGameObject.transform.localPosition = SnapToGrid( localPoint - (Vector2)oddOffset, tileSize, tileSize) + oddOffset;
            if(VeryBadlyImplementedCollistionCheck(dragItemHandle)){
                dragItemHandle.background.color = Color.red;
            }
            else {
                dragItemHandle.background.color = Color.white;
            }
        }
    }



    public void OnPointerDown(PointerEventData poi){
        // Check for valid GameObject (item) to drag
        if(poi.button == PointerEventData.InputButton.Left) {
            GameObject hitGameObject = poi.pointerCurrentRaycast.gameObject;
            if(hitGameObject.tag == "InventoryItem"){
                dragGameObject = hitGameObject;
                dragItemHandle = dragGameObject.GetComponent<KuroItemHandle>();
                dragGameObject.transform.SetAsLastSibling();
            }
        }
    }
    public void OnPointerUp(PointerEventData poi){

        // Forget about dragged GameObject
        dragGameObject = null;
        dragItemHandle = null;
    }

    private bool VeryBadlyImplementedCollistionCheck(KuroItemHandle item){
        foreach(KuroItemHandle otherItem in items){
            if(item == otherItem) continue;
            if(RectOverlap(item.rectTransform, item.rotated, otherItem.rectTransform, otherItem.rotated)) return true;
        }
        return false;
    }

    private bool RectOverlap(RectTransform firstRect, bool firstRotated, RectTransform secondRect, bool secondRotated) // rect.Overlap() is thrash
    {
        float firstWidth = !firstRotated ? firstRect.rect.width : firstRect.rect.height;
        float secondWidth = !secondRotated ? secondRect.rect.width : secondRect.rect.height;
        float firstHeight = firstRotated ? firstRect.rect.width : firstRect.rect.height;
        float secondHeight = secondRotated ? secondRect.rect.width : secondRect.rect.height;
        if (firstRect.localPosition.x + firstWidth * 0.5f < secondRect.localPosition.x - secondWidth * 0.5f)
        {
            return false;
        }
        if (secondRect.localPosition.x + secondWidth * 0.5f < firstRect.localPosition.x - firstWidth * 0.5f)
        {
            return false;
        }
        if (firstRect.localPosition.y + firstHeight * 0.5f < secondRect.localPosition.y - secondHeight * 0.5f)
        {
            return false;
        }
        if (secondRect.localPosition.y + secondHeight * 0.5f < firstRect.localPosition.y - firstHeight * 0.5f)
        {
            return false;
        }
        return true;
    }

    private KuroItemHandle AddItemTest(KuroItem itemData){
        GameObject newItem = Instantiate(itemPrefab, transform);
        newItem.name = itemData.itemName;
        KuroItemHandle itemHandle = newItem.GetComponent<KuroItemHandle>();
        itemHandle.LoadData(itemData);
        itemHandle.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryBounds.width / gridSize.x * itemData.size.x - .5f);
        itemHandle.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryBounds.width / gridSize.x * itemData.size.y - .5f);
        return itemHandle;
    }


    private Vector3 SnapToGrid(Vector3 input, float xMult, float yMult, float zMult = 1f) { // "Rounds" to the nearest point on a grid
        // input.x  xMult   output.x
        // 2.3      1       2
        // 2.3      10      0
        // 5.6      10      10
        // 5.1      50      0
        
        //Debug.LogFormat("input: {0}\nmult: {1}\nBeforeRound: {2}\nAfterRound: {3}\noutput: {4}\n", input.x, xMult, input.x / xMult, Mathf.Round(input.x / xMult), Mathf.Round(input.x / xMult) * xMult);

        Vector3 output = Vector3.zero;
        output.x = Mathf.Round(input.x / xMult) * xMult;
        output.y = Mathf.Round(input.y / yMult) * yMult;
        output.z = Mathf.Round(input.z / zMult) * zMult;

        return output;
    }
}
