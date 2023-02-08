using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryFunction : MonoBehaviour
{
    //velikost (šířka a výška) item slotu 
    [SerializeField] private float itemSlotSize;
    //padding okna s item sloty
    [SerializeField] private float padding;
    private Button button;
    //obsah inventáře
    private GameObject[,] inventoryContent;
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(DebugInv);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DebugInv()
    {
        Debug.DrawLine(gameObject.transform.position, Input.)
    }
}
