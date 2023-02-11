using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timescale : MonoBehaviour
{
    [SerializeField] [Range(0f,2f)] private float setTimeScale = 1;

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = setTimeScale;
    }
}
