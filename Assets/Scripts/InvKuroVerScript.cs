using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InvKuroVerScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{


    public GameObject movingGameObject;


    private int fps = 0;
    private float fpsRefresh = 0f;

    private RectTransform invRect;

    void Start(){
        invRect = gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        fpsRefresh += Time.deltaTime;
        if(fpsRefresh > 0.5f){
            fps = (int)Mathf.Pow(Time.deltaTime, -1);
            fpsRefresh = 0f;
        }
        Vector2 localPoint;
        if(movingGameObject && RectTransformUtility.ScreenPointToLocalPointInRectangle(invRect, Input.mousePosition, Camera.current, out localPoint)){
            movingGameObject.transform.localPosition = SnapToGrid( localPoint, 100f, 100f );
        }
    }

    public void OnPointerDown(PointerEventData poi){
        if(poi.button == PointerEventData.InputButton.Left) {
            GameObject hitGameObject = poi.pointerCurrentRaycast.gameObject;
            if(hitGameObject.tag == "InventoryItem"){
                movingGameObject = hitGameObject;
                movingGameObject.transform.SetAsLastSibling();
            }
        }
    }

    private Vector3 SnapToGrid(Vector3 input, float xMult, float yMult, float zMult = 1f) { // "Rounds" to the nearest point on a grid
        // input.x  xMult   output.x
        // 2.3      1       2
        // 2.3      10      0
        // 5.6      10      10
        // 5.1      50      0
        
        //Debug.LogFormat("input: {0}\nmult: {1}\nBeforeRound: {2}\nAfterRound: {3}\noutput: {4}\n", input.x, xMult, input.x / xMult, Mathf.Round(input.x / xMult), Mathf.Round(input.x / xMult) * xMult);

        Vector3 output = Vector3.zero;
        output.x = Mathf.Round(input.x / xMult) * xMult;
        output.y = Mathf.Round(input.y / yMult) * yMult;
        output.z = Mathf.Round(input.z / zMult) * zMult;

        return output;
    }
    
    public void OnPointerUp(PointerEventData poi){
        movingGameObject = null;
    }
    
    private void OnGUI() {
        GUIStyle style = new GUIStyle();
        style.richText = true;
        GUILayout.Label("<size=30>" + fps.ToString() + " fps</size>",style);
    }
}
