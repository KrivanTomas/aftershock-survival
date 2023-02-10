using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInventoryFunction : MonoBehaviour
{
    // Start is called before the first frame update
    public InventoryFunction invScript;
    public Item[] addInventory;
    public Item adding;
    public int index = 0;
    public bool delej = false;
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        if(delej)
        {
            invScript.Pridej(adding,1,0);
            index++;
            delej = false;
        }
    }
}
