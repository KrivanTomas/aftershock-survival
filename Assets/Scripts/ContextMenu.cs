using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContextMenu : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject contx;

    private InventoryControl inventoryControl;
    private void Start(){
        inventoryControl = transform.parent.parent.GetComponent<InventoryControl>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            contx.SetActive(true);
        else 
            contx.SetActive(false);
    }

    public void Equip(){
        inventoryControl.OpenClose(false);
        inventoryControl.advWeapon.EquipPistol();
    }
}
