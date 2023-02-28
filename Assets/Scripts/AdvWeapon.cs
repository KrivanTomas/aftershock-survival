using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvWeapon : MonoBehaviour
{
    [SerializeField] private Transform weapon;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform hipPosition;
    [SerializeField] private Transform aimPosition;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip gunshot;

    [Header("Settings")]

    [SerializeField] private Vector3 weaponKickPos = Vector3.up;
    [SerializeField] private Vector3 weaponKickRot = Vector3.up;
    [SerializeField] private Vector3 cameraKickRot = Vector3.zero;


    private Vector3 aimVelocity = Vector3.zero;
    [SerializeField] private float aimTime = 1f;
    [SerializeField] private float aimMaxSpeed = 1f;


    [SerializeField] private float angleDelta = 10f;




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
            weapon.position += weapon.transform.TransformVector(weaponKickPos);
            weapon.rotation *= Quaternion.Euler(weaponKickRot);
            cameraTransform.rotation *= Quaternion.Euler(cameraKickRot);
            audioSource.pitch = Random.Range(1f,1.1f);
            audioSource.PlayOneShot(gunshot);
        }
        cameraTransform.localRotation = Quaternion.RotateTowards(cameraTransform.localRotation, Quaternion.Euler(0f,0f,0f), angleDelta * Quaternion.Angle(cameraTransform.localRotation, Quaternion.Euler(0f,0f,0f)) * 0.36f);
        weapon.position = Vector3.SmoothDamp(weapon.position, finalPos.position, ref aimVelocity, aimTime, aimMaxSpeed * (isAiming ? 10 : 1) * Vector3.Distance(weapon.position, finalPos.position));
        weapon.rotation = Quaternion.RotateTowards(weapon.rotation, finalPos.rotation, angleDelta * Quaternion.Angle(weapon.rotation, finalPos.rotation));
    }
}
