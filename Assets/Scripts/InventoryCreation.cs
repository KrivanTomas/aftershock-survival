using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCreation : MonoBehaviour
{
    //představuje inventář jako celek - spravuje item sloty
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject itemSlot;
    private RectTransform invtrans;
    private RectTransform itmtrans;
    private GridLayoutGroup gridLayout;
    public bool otestuj = false;
    public int PocetSlotySirka;
    public int PocetSlotyVyska;
    void Start()
    {
        invtrans = inventory.GetComponent<RectTransform>();
        itmtrans = itemSlot.GetComponent<RectTransform>();
        gridLayout = inventory.GetComponent<GridLayoutGroup>();
    }
    private void Update() {
        if(otestuj)
        Inicializuj();
        otestuj = false;
        //Debug.Log(invtrans.rect);
    }
    private void Inicializuj()
    {
        //vymazání
        for(int i = 0;i<inventory.transform.childCount;i++)
        {
            Destroy(inventory.transform.GetChild(i).gameObject);
        }
        //nastavení velikosti
        //[left;top] - počáteční bod okna (RectangleTransform)
        //(right - left) - šířka okna
        //(bottom - top) - výška okna
        
        //vytvoření slotů inventáře
        
        int pocet = PocetSlotySirka*PocetSlotyVyska;
        for(int i = 0;i<pocet;i++)
        {
            Instantiate(itemSlot,inventory.transform);            
           
        }
        
        //nastavení velikosti okna inventáře
        /*
        šířka = gridLayout.padding.horizontal + itmtrans.rect.width * PocetSlotySirka;
        výška = gridLayout.padding.vertical + itmtrans.rect.height * PocetSlotyVyska;
        */
        invtrans.sizeDelta = new Vector2(gridLayout.padding.horizontal + itmtrans.rect.width * PocetSlotySirka,gridLayout.padding.vertical + itmtrans.rect.height * PocetSlotyVyska);
        
    }
}
