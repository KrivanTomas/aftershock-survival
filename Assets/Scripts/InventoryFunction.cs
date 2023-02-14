using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
public class InventoryFunction : MonoBehaviour
{
    //todo:     práce se souřadnicema
    //          ošetření index out of range v metodě Pridej / Odeber (příliš velký item)
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
    //to co je na kurzoru při přesouvání itemů
    [SerializeField] private CursorFollow cursorFollow;
    //strukturovaný datový typ co ukládá informace o itemu zbytečné pro samotnou třídu Item
    //předpokládám že informace kde je item umístěn nebude potřeba komunikovat zvenčí
    //*****potom nastavit na private*****************************************************************************************************************************************************************************
    public struct ItemInfo
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
        int x, y;
        ItemInfo item;
        if(Input.GetMouseButtonDown(0))
        {
            if(ZiskejKliknuty(out x, out y))
            {
                item = new ItemInfo(inventoryContent[x,y],x,y);
                Odeber(item.item);
                cursorFollow.NastavItem(item);
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            if(ZiskejKliknuty(out x, out y) && !InventoryFunction.ItemInfo.Equals(cursorFollow.Obsahuje, new InventoryFunction.ItemInfo()))
            {
                if(Pridej(cursorFollow.Obsahuje.item,x,y))
                {
                    cursorFollow.Vypni();
                }
                else
                {
                    VratKurzor();
                }
            }
            else
            {
                VratKurzor();
            }
        }
        
    }
    //jako vrátit do původního stavu
    public void VratKurzor()
    {
        if(!Pridej(cursorFollow.Obsahuje.item,cursorFollow.Obsahuje.pozX,cursorFollow.Obsahuje.pozY))
        {
            for(int i = 0;i<inventoryContent.GetLength(0);i++)
            {
                for(int j = 0;j<inventoryContent.GetLength(1);j++)
                {
                    Pridej(cursorFollow.Obsahuje.item,i,j);
                }
            }
        }
        cursorFollow.Vypni();
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

    
    //pokusí se přidat item do inventáře
    //vrací bool podle úspěšnosti
    public bool Pridej(Item item, int pozX, int pozY)
    {
        if(pozX < 0 || pozY < 0 || pozX + item.SizeX >= inventoryContent.GetLongLength(0) || pozY + item.SizeY >= inventoryContent.GetLength(1))
            return false;
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
    private bool ZiskejKliknuty(out int x, out int y)
    {
        x = 0; y = 0;
        Vector3 kurzor = Input.mousePosition - inventoryLocation.transform.position;
        if((float)PocetSirka * itemSlotSize / 2 < Mathf.Abs(kurzor.x) 
        || (float)PocetVyska * itemSlotSize / 2 < Mathf.Abs(kurzor.y))
        {
            return false;
        }
        //výchozí pozice levý horní roh
        kurzor += new Vector3((float)PocetSirka * itemSlotSize / 2, -(float)PocetVyska * itemSlotSize / 2, 0);
        //převod na souřadnice v poli
        x = Mathf.FloorToInt(kurzor.x / itemSlotSize); y = Mathf.FloorToInt(-kurzor.y / itemSlotSize);
        if(inventoryContent[x,y] == null)
        {
            return false;
        }
        return true;
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
