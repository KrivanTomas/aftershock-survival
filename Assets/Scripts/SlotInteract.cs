using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SlotInteract : MonoBehaviour
{
    //odkaz na item který itemSlot obsahuje
    public GameObject Item;
    //zda item slot obsahuje item
    public bool ObsahujeItem{get;private set;}
    [SerializeField] private RectTransform slotTransform;
    [SerializeField] private Button slotButton;

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(slotTransform.position,Input.mousePosition,Color.red);
    }
}
