using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvWeapon : MonoBehaviour
{
    [SerializeField] private Transform weapon;
    [SerializeField] private Transform weaponFloat;
    [SerializeField] private Transform hipPosition;
    [SerializeField] private Transform aimPosition;

    [Header("Settings")]

    [SerializeField] private Vector3 weaponKick = Vector3.up;


    private Vector3 aimVelocity = Vector3.zero;
    [SerializeField] private float aimTime = 1f;
    [SerializeField] private float aimMaxSpeed = 1f;


    private bool isAiming = false;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        Transform finalPos = hipPosition;
        isAiming = false;
        if(Input.GetMouseButton(1)) {
            finalPos = aimPosition;
            isAiming = true;
        }

        if(Input.GetMouseButtonDown(0)){
            weapon.position += weapon.transform.TransformVector(weaponKick);
        }

        weapon.position = Vector3.SmoothDamp(weapon.position, finalPos.position, ref aimVelocity, aimTime, aimMaxSpeed * (isAiming ? 10 : 1));
        weapon.rotation = finalPos.rotation;
    }
}
