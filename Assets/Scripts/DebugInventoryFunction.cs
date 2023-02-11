using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInventoryFunction : MonoBehaviour
{
    // Start is called before the first frame update
    public InventoryFunction invScript;
    public Item[] addInventory;
    public int[] poziceX;
    public int[] poziceY;
    public bool pridej = false;
    public bool vymaz = false;
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        if(pridej)
        {
            for(int i = 0;i<addInventory.Length;i++)
            {
                invScript.Pridej(addInventory[i],poziceX[i],poziceY[i]);
            }
            pridej = false;
        }
        if(vymaz)
        {
            for(int i = 0;i<addInventory.Length;i++)
            {
                invScript.Odeber(addInventory[i]);
            }
            vymaz = false;
        }
    }
}
