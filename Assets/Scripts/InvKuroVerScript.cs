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
    [SerializeField] private KuroItem[] testItemData;    

    [SerializeField] private Color defaultColor;
    [SerializeField] private Color collisionColor;

    private Vector3 whtOffset;

    private float tileSize;

    private KuroItemHandle contextItem;
    private GameObject dragGameObject;
    private KuroItemHandle dragItemHandle;
    private RectTransform inventoryRectTransform;
    private Vector3 lastPosition;
    private bool lastRotation = false;
    private bool collided = false;

    private Rect inventoryBounds;


    private List<KuroItemHandle> items = new List<KuroItemHandle>();

    void Start(){
        inventoryRectTransform = gameObject.GetComponent<RectTransform>();
        inventoryBounds = inventoryRectTransform.rect;
        tileSize = inventoryBounds.size.x / gridSize.x;
        whtOffset = new Vector3(tileSize * .5f, inventoryBounds.height * .5f + tileSize * .5f, 0f);

        // for(int i = 0; i < 10; i++){
        //     KuroItemHandle itemHandle;
        //     if(TryAddItem(testItemData[Random.Range(0, testItemData.GetLength(0) - 1)], out itemHandle)){
        //         items.Add(itemHandle);
        //     }
        //     else{
        //         Debug.Log("<color=red>Item dosent fit " + i + "</color>");
        //     }
            
        // }
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Semicolon)){
            KuroItemHandle itemHandle;
            if(TryAddItem(testItemData[Random.Range(0, testItemData.GetLength(0) - 1)], out itemHandle)){
                items.Add(itemHandle);
            }
        }
        // Move dragged GameObject while snapping to grid
        Vector2 localPoint;
        
        if(dragGameObject && RectTransformUtility.ScreenPointToLocalPointInRectangle(inventoryRectTransform, Input.mousePosition, Camera.current, out localPoint)){
            // Rotate the item
            if(Input.GetKeyDown(KeyCode.R)){
                dragItemHandle.Rotate();
            }
            // Shift the grid for odd axis
            Vector3 oddOffset = CalculateOddOffset(dragItemHandle);

            dragGameObject.transform.localPosition = SnapToGrid( localPoint - (Vector2)oddOffset - (Vector2)whtOffset, tileSize, tileSize) + oddOffset + whtOffset;
            if(VeryBadlyImplementedCollistionCheck(dragItemHandle) || OutOfBoundsCheck(dragItemHandle, inventoryRectTransform)){
                collided = true;
                dragItemHandle.background.color = collisionColor;
            }
            else {
                collided = false;
                dragItemHandle.background.color = defaultColor;
            }
        }
    }

    public void OnPointerDown(PointerEventData poi){
        // Check for valid GameObject (item) to drag
        collided = false;
        if(poi.button == PointerEventData.InputButton.Left) {
            if(contextItem){
                contextItem.HideContextMenu();
                contextItem = null;
            }
            GameObject hitGameObject = poi.pointerCurrentRaycast.gameObject;
            if(hitGameObject.tag == "InventoryItem"){
                dragGameObject = hitGameObject;
                dragItemHandle = dragGameObject.GetComponent<KuroItemHandle>();
                lastPosition = dragGameObject.transform.position;
                lastRotation = dragItemHandle.rotated;
                dragGameObject.transform.SetAsLastSibling();
            }
        }
        else if(poi.button == PointerEventData.InputButton.Right) {
            if(contextItem){
                contextItem.HideContextMenu();
                contextItem = null;
            }
            GameObject hitGameObject = poi.pointerCurrentRaycast.gameObject;
            if(hitGameObject.tag == "InventoryItem"){
                contextItem = hitGameObject.GetComponent<KuroItemHandle>();
                contextItem.transform.SetAsLastSibling();
                contextItem.ShowContextMenu();
            }
        }
    }
    public void OnPointerUp(PointerEventData poi){
        if(collided && dragGameObject){
            dragGameObject.transform.position = lastPosition;
            dragItemHandle.background.color = defaultColor;
            dragItemHandle.Rotate(lastRotation);
        }
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

    private bool OutOfBoundsCheck(KuroItemHandle itemHandle, RectTransform inventoryTransform){
        return !RectContains(inventoryRectTransform, itemHandle.rectTransform, itemHandle.rotated);
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

    private bool RectContains(RectTransform containerRect, RectTransform containedRect, bool containedRotated)
    {
        float firstWidth = containerRect.rect.width;
        float firstHeight = containerRect.rect.height;
        float secondWidth = !containedRotated ? containedRect.rect.width : containedRect.rect.height;
        float secondHeight = containedRotated ? containedRect.rect.width : containedRect.rect.height;
        
        if (containedRect.localPosition.x + secondWidth * .5f > firstWidth * .5f || containedRect.localPosition.x - secondWidth * .5f < -firstWidth * .5f)
        {
            return false;
        }
        if (containedRect.localPosition.y + secondHeight * .5f > firstHeight * .5f || containedRect.localPosition.y - secondHeight * .5f < -firstHeight * .5f)
        {
            return false;
        }
        return true;
    }

    private void RemoveItem(KuroItemHandle itemHandle){
        items.Remove(itemHandle);
        Destroy(itemHandle.gameObject);
    }

    public bool TryAddItem(KuroItem itemData, out KuroItemHandle itemHandle){
        // speed up conditions:
        //  - in inv bounds
        //  - not in the same position
        //  -

        itemHandle = AddItem(itemData);

        Vector3 oddOffset = 
                Vector3.right * (itemHandle.finalSize.x % 2 == 0 ? tileSize * 0.5f : 0f) +
                Vector3.up * (itemHandle.finalSize.y % 2 == 0 ? tileSize * 0.5f : 0f);

        for(int row = 0; row < gridSize.x; row++){
            for(int column = 0; column < gridSize.x; column++){ 
                Vector3 scanPos = new Vector3(tileSize * column - inventoryBounds.width * .5f + tileSize * .5f, tileSize * -row + inventoryBounds.height * .5f - tileSize * .5f, 0f);
                itemHandle.rectTransform.localPosition = scanPos + CalculateOddOffset(itemHandle);
                if(!VeryBadlyImplementedCollistionCheck(itemHandle) && !OutOfBoundsCheck(itemHandle, inventoryRectTransform)){
                    return true;
                }
                else{
                    itemHandle.Rotate();
                    itemHandle.rectTransform.localPosition = scanPos + CalculateOddOffset(itemHandle);
                    if(!VeryBadlyImplementedCollistionCheck(itemHandle) && !OutOfBoundsCheck(itemHandle, inventoryRectTransform)){
                        return true;
                    }
                    itemHandle.Rotate();
                }
            }
        }
        Destroy(itemHandle.gameObject);
        return false;        
    }

    private KuroItemHandle AddItem(KuroItem itemData){
        GameObject newItem = Instantiate(itemPrefab, transform);
        newItem.name = itemData.itemName;
        KuroItemHandle itemHandle = newItem.GetComponent<KuroItemHandle>();
        itemHandle.LoadData(itemData);
        itemHandle.background.color = defaultColor;
        itemHandle.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryBounds.width / gridSize.x * itemData.size.x - .5f);
        itemHandle.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryBounds.width / gridSize.x * itemData.size.y - .5f);
        return itemHandle;
    }

    private Vector3 CalculateOddOffset(KuroItemHandle itemHandle){
        Vector3 oddOffset = 
                Vector3.right * (itemHandle.finalSize.x % 2 == 0 ? tileSize * 0.5f : 0f) +
                Vector3.up * (itemHandle.finalSize.y % 2 == 0 ? tileSize * 0.5f : 0f);
        return oddOffset;
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
