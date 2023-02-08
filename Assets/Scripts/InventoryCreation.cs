using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCreation : MonoBehaviour
{
    //hlavní objekt inventáře
    [SerializeField] 
    private RectTransform inventoryParent;
    //skript vytváří tento objekt
    [SerializeField] 
    private GameObject itemSlotChild;
    //kam skript vytváří item sloty
    [SerializeField] 
    private RectTransform itemSlotParent;
    private GridLayoutGroup gridLayout;
    public bool otestuj = false;
    public int PocetSlotySirka;
    public int PocetSlotyVyska;
    void Start()
    {
        gridLayout = itemSlotParent.GetComponent<GridLayoutGroup>();
    }
    private void Update() {
        if(otestuj)
        Inicializuj();
        otestuj = false;

    }
    private void Inicializuj()
    {
        //vymazání
        for(int i = 0;i<itemSlotParent.childCount;i++)
        {
            Destroy(itemSlotParent.GetChild(i).gameObject);
        }
        //nastavení velikosti
        //[left;top] - počáteční bod okna (RectangleTransform)
        //(right - left) - šířka okna
        //(bottom - top) - výška okna
        
        //vytvoření slotů inventáře
        
        int pocet = PocetSlotySirka*PocetSlotyVyska;
        for(int i = 0;i<pocet;i++)
        {
            Instantiate(itemSlotChild,itemSlotParent);            
           
        }
        
        //nastavení velikosti okna inventáře
        /*
        šířka = gridLayout.padding.horizontal + itmtrans.rect.width * PocetSlotySirka;
        výška = gridLayout.padding.vertical + itmtrans.rect.height * PocetSlotyVyska;
        */
        inventoryParent.sizeDelta = new Vector2(
            gridLayout.padding.horizontal + (itemSlotChild.transform as RectTransform).rect.width * PocetSlotySirka,
            gridLayout.padding.vertical + (itemSlotChild.transform as RectTransform).rect.height * PocetSlotyVyska);
        
    }
}
