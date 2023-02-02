using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryCreation : MonoBehaviour
{
    //představuje inventář jako celek - spravuje item sloty
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject itemSlot;
    private GridLayoutGroup grid;

    public bool otestuj = false;
    public int PocetSlotySirka;
    public int PocetSlotyVyska;
    void Start()
    {
    }
    private void Update() {
        if(otestuj)
        Inicializuj();
        otestuj = false;
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
        
        //vytvoření
        int pocet = PocetSlotySirka*PocetSlotyVyska;
        for(int i = 0;i<pocet;i++)
            Instantiate(itemSlot,inventory.transform);
        
    }
}
