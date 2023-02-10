using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryFunction : MonoBehaviour
{
    //todo: změna velikosti inv během hry
    //      práce s listem (ItemInfo -> Item)
    //      nastavení velikosti -> změnit pozici
    //      lépe zakomentovat

    //velikost (šířka a výška) item slotu 
    public int PocetVyska;
    public int PocetSirka;
    [SerializeField] private float itemSlotSize;
    //padding okna s item sloty
    [SerializeField] private float padding;
    [SerializeField] private RectTransform inventoryLocation;
    [SerializeField] private GameObject itemPrefab;

    private struct ItemInfo
    {
        public ItemInfo(Item item, int pozX, int pozY)
        {
            this.item = item;
            this.pozX = pozX;
            this.pozY = pozY;
        }
        public int pozX;
        public int pozY;
        public Item item;
    }

    //pomocí tohoto pole (a itemSlotSize, padding) se podle pozice kurzoru dopočítá na jaký slot hráč klikl
    private Item[,] inventoryContent;
    //seznam všech itemů v inventáři
    private List<ItemInfo> ItemList;

    
    void Start()
    {
        inventoryContent = new Item[PocetSirka,PocetVyska];
        ItemList = new List<ItemInfo>();
    }
    public Item[] Inventar()
    {
        //return ItemList.ToArray();
        return null;

    }
    //horní levý roh
    public bool Pridej(Item item, int pozX, int pozY)
    {
        //kontrola
        for(int i = 0;i<item.SizeX;i++)
        {
            for(int j = 0;j<item.SizeY;j++)
            {
                if(inventoryContent[pozX + i, pozY + j] != null)
                    return false;
            }
        }
        //nastavení hodnot
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
    public void Odeber(Item item)
    {
        //ItemInfo itemInfo = ItemList.Find();
        for(int i = 0;i<item.SizeX;i++)
        {
            for(int j = 0;j<item.SizeY;j++)
            {
                //inventoryContent[pozX + i, pozY + j] = null;
            }
        }
    }
    //vymaže všechny objekty z inventoryLocation a vykreslí itemy podle inventoryContent
    private void Vykresli()
    {
        //vymazání
        for(int i = 0;i<inventoryLocation.childCount;i++)
        {
            Destroy(inventoryLocation.GetChild(i).gameObject);
        }
        //vykreslení
        foreach(ItemInfo itemInfo in ItemList)
        {
            GameObject slot = Instantiate(itemPrefab,inventoryLocation);
            slot.GetComponent<RawImage>().texture = itemInfo.item.Icon;
            //nastavení pozice objektu
            //itemSlotSize / 2 - výchozí pozice levý horní roh - (kvůli nastavení anchor)
            //padding - mezery okna inventáře
            //itemSlotSize * itemInfo.pozX - rozdělení na čtvercové pole
            (slot.transform as RectTransform).anchoredPosition = new Vector2(
                itemSlotSize / 2 + padding + itemSlotSize * itemInfo.pozX,
                -itemSlotSize / 2 - padding - itemSlotSize * itemInfo.pozY);
            //nastavení velikosti objektu
            //(slot.transform as RectTransform).sizeDelta;
        }
            
    }
}
