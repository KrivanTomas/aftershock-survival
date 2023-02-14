using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryFunction : MonoBehaviour
{
    //todo:     práce se souřadnicema
    //          ošetření index out of range
    //          optimalizace práce s listem, vykreslení
    
    public int PocetVyska;
    public int PocetSirka;
    //velikost (šířka a výška) item slotu v grafickém zobrazení
    [SerializeField] private float itemSlotSize;
    //velikost okraje okna inventáře v grafickém zobrazení
    [SerializeField] private float padding;
    //objekt pod který se vykreslují položky v inventáři
    [SerializeField] private RectTransform inventoryLocation;
    //objekt pomocí kterého se vykreslují položky v inventáři
    [SerializeField] private GameObject itemPrefab;
    //strukturovaný datový typ co ukládá informace o itemu zbytečné pro samotnou třídu Item
    //předpokládám že informace kde je item umístěn nebude potřeba komunikovat zvenčí
    private struct ItemInfo
    {
        public ItemInfo(Item item, int pozX, int pozY)
        {
            this.item = item;
            this.pozX = pozX;
            this.pozY = pozY;
        }
        public Item item;
        //pozice itemu v inventáři
        //pokud větší než 1x1 tak levý horní roh itemu
        public int pozX;
        public int pozY;
        
    }

    //současný stav inventáře jako čtvercové pole
    //slouží jen pro kontrolu
    private Item[,] inventoryContent;
    //seznam všech itemů v inventáři
    private List<ItemInfo> ItemList;

    
    void Start()
    {
        inventoryContent = new Item[PocetSirka,PocetVyska];
        ItemList = new List<ItemInfo>();
    }
    void Update()
    {
        //Debug.Log(Input.mousePosition - inventoryLocation.position);
        Vector2 slotsSize = new Vector2(400,400);
        Debug.Log(slotsSize);
    }
    // vrátí pole všech itemů v inventáři
    public Item[] Inventar()
    {
        int pocet = ItemList.Count;
        Item[] items = new Item[pocet];
        for(int i = 0;i<pocet;i++)
        {
            items[i] = ItemList[i].item;
        }
        return items;

    }
    public bool Klikni(Vector3 kurzor,out GameObject item,out Vector3 offset)
    {
        item = null;
        offset = new Vector2();
        //mimo
        if(Mathf.Abs(kurzor.x) > (float)PocetSirka / 2 * itemSlotSize ||
        Mathf.Abs(kurzor.y) > (float)PocetVyska)
        {
            return false;
        }
        //prazdny
        

        return true;
    }
    //pokusí se přidat item do inventáře
    //vrací bool podle úspěšnosti
    public bool Pridej(Item item, int pozX, int pozY)
    {
        //kontrola platnosti umístění
        for(int i = 0;i<item.SizeX;i++)
        {
            for(int j = 0;j<item.SizeY;j++)
            {
                if(inventoryContent[pozX + i, pozY + j] != null)
                    return false;
            }
        }
        //uložení, vykreslení
        for(int i = 0;i<item.SizeX;i++)
        {
            for(int j = 0;j<item.SizeY;j++)
            {
                inventoryContent[pozX + i, pozY + j] = item;
            }
        }
        ItemList.Add(new ItemInfo(item,pozX,pozY));
        Vykresli();
        return true;
    }
    //odebere item z inventáře
    public void Odeber(Item item)
    {
        //vyhledání ItemInfo v listu podle parametru
        ItemInfo itemInfo = new ItemInfo();
        foreach(ItemInfo polozka in ItemList)
        {
            if(polozka.item.Id == item.Id)
            {
                itemInfo = polozka;
                break;
            }
        }
        //vymazání, vykreslení
        for(int i = 0;i<item.SizeX;i++)
        {
            for(int j = 0;j<item.SizeY;j++)
            {
                inventoryContent[itemInfo.pozX + i, itemInfo.pozY + j] = null;
            }
        }
        ItemList.Remove(itemInfo);
        Vykresli();
    }
    //vymaže všechny objekty z inventoryLocation a vykreslí itemy podle ItemList
    private void Vykresli()
    {
        //vymazání předchozích objektů
        for(int i = 0;i<inventoryLocation.childCount;i++)
        {
            Destroy(inventoryLocation.GetChild(i).gameObject);
        }
        //vykrelí současný stav
        foreach(ItemInfo itemInfo in ItemList)
        {
            GameObject slot = Instantiate(itemPrefab,inventoryLocation);
            slot.GetComponent<RawImage>().texture = itemInfo.item.Icon;
            //nastavení pozice objektu
            //padding - mezery okna inventáře
            //itemSlotSize * itemInfo.pozX - rozdělení na čtvercové pole
            (slot.transform as RectTransform).anchoredPosition = new Vector2(
                padding + itemSlotSize * itemInfo.pozX,
                padding - itemSlotSize * itemInfo.pozY);
            //nastavení velikosti objektu
            //pivot[0,1]
            (slot.transform as RectTransform).sizeDelta = new Vector2(
                itemSlotSize * itemInfo.item.SizeX,
                itemSlotSize * itemInfo.item.SizeY);
        }
            
    }
}
