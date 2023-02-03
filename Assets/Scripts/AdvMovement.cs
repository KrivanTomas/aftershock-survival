using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvMovement : MonoBehaviour
{
    // Advanced movement script

    // walking, sprinting, crouching, crawl, vault, climb

    // todo:

    // air movement?
    // fix strayfing speed advantage
    // tweening?


    [Header("References")]
    [SerializeField] private GameObject playerGameObject;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform view;

    [Header("Settings")]
    [SerializeField] private float baseSpeed = 10f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float jumpHeight = 0.5f;
    [SerializeField] [Range(0f,1f)] private float airMovement = 0f;
    [SerializeField] private float groundDistanse = 0.1f;
    [SerializeField] private LayerMask groundMask;



    

    // Helper variables
    [SerializeField] private Vector3 velocity;
    private Vector3 movement;
    private float verticalRotation = 0f;
    private bool isGrounded = false;
    
    private void Temp() {
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void Start() {
        Temp();
    }

    private void Update() {
        MoveView();
        MovePlayer();
    }   

    private void MoveView() {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation,-90f,90f);

        view.transform.localRotation = Quaternion.Euler(verticalRotation,0f,0f);
        playerGameObject.transform.eulerAngles += new Vector3(0f,mouseX,0f);
    }

    private void MovePlayer() {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistanse, groundMask);

        Vector3 inputMovement = Input.GetAxis("Vertical") * transform.forward + Input.GetAxis("Horizontal") * transform.right;
        inputMovement *= baseSpeed * Time.deltaTime;

        if(isGrounded && velocity.y < 0){
            velocity.y = -0.02f;
            movement = inputMovement;
        }

        // if (isGrounded) {
            
        // }
        // else {
        //     movement += inputMovement * airMovement;
        //     movement.x = Mathf.Clamp(movement.x, inputMovement.x * airMovement,inputMovement.x * airMovement);
        //     movement.z = Mathf.Clamp(movement.z, inputMovement.z * airMovement,inputMovement.z * airMovement);
        // }

        if (isGrounded && velocity.y < 0 && Input.GetButtonDown("Jump")) {
            velocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime + movement);
    }
}
