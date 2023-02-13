using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryControl : MonoBehaviour
{
    public KeyCode CloseOpenKey;
    public GameObject InventoryParent;
    public float ItemSlotSize;
    public bool InventoryOpen;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(CloseOpenKey))
        {
            InventoryOpen = !InventoryOpen;
            InventoryParent.transform.GetChild(0).gameObject.SetActive(InventoryOpen);
            InventoryParent.transform.GetChild(1).gameObject.SetActive(InventoryOpen);
        }
    }
}
