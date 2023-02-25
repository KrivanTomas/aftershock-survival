using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KuroItemHandle : MonoBehaviour
{
    public int itemID;
    public string itemName;
    public string itemDescription;
    public Vector2 itemSize;
    public Vector2 rotatedItemSize;
    public Vector2 finalSize;


    [SerializeField] public RectTransform rectTransform;
    [SerializeField] public Image background;
    [SerializeField] private Image image;
    public bool rotated = false;

    public void LoadData(KuroItem data){
        itemID = data.itemID;
        itemName = data.itemName;
        itemDescription = data.itemDescription;
        itemSize = data.size;
        rotatedItemSize = new Vector2(data.size.y, data.size.x);
        finalSize = itemSize;
        SetSprite(data.sprite);
    }

    public void SetSprite(Sprite sprite){
        image.sprite = sprite;
    }

    public void Rotate(){
        rotated = !rotated;
        rectTransform.rotation = !rotated ? Quaternion.Euler(0f,0f,0f) : Quaternion.Euler(0f,0f,90f);
        rectTransform.ForceUpdateRectTransforms();
        finalSize = !rotated ? itemSize : rotatedItemSize;
    }
    public void Rotate(bool forceRotationState){
        rotated = forceRotationState;
        rectTransform.rotation = !rotated ? Quaternion.Euler(0f,0f,0f) : Quaternion.Euler(0f,0f,90f);
        finalSize = !rotated ? itemSize : rotatedItemSize;
    }
}
