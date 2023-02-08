using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryFunction : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float itemSlotSize;
    [SerializeField] private float padding;
    private Button button;

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
