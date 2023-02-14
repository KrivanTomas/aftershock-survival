using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryControl : MonoBehaviour
{
    [SerializeField] private InventoryFunction inventoryFunction;
    public KeyCode CloseOpenKey;
    public GameObject InventoryParent;
    [SerializeField] private AdvMovement advMovement;
    public AdvWeapon advWeapon;
    public float ItemSlotSize;
    public bool InventoryOpen;
    public bool Test = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(CloseOpenKey))
        {
            InventoryOpen = !InventoryOpen;
            advMovement.Shut(InventoryOpen);
            advWeapon.Shut(InventoryOpen);
            InventoryParent.transform.GetChild(0).gameObject.SetActive(InventoryOpen);
            InventoryParent.transform.GetChild(1).gameObject.SetActive(InventoryOpen);
            if(!InventoryOpen)
            inventoryFunction.VratKurzor();
        }
        
    }

    public void OpenClose(bool idk){
        InventoryOpen = idk;
        advMovement.Shut(InventoryOpen);
        advWeapon.Shut(InventoryOpen);
        InventoryParent.transform.GetChild(0).gameObject.SetActive(InventoryOpen);
        InventoryParent.transform.GetChild(1).gameObject.SetActive(InventoryOpen);
        if(!InventoryOpen)
        inventoryFunction.VratKurzor();
    }
}
