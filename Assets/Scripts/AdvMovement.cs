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
    [SerializeField] [Range(0f,1f)] private float cameraOffset = 0.3f;
    [SerializeField] private float baseHeight = 1.75f;
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private float crouchHeight = .8f;
    [SerializeField] private float crouchSpeed = 2f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float staticGravity = -0.2f;
    [SerializeField] private float jumpHeight = 0.5f;
    //[SerializeField] [Range(0f,1f)] private float airMovement = 0f;
    [SerializeField] private float groundDistanse = 0.1f;
    [SerializeField] private float groundOffset = 0.1f;
    [SerializeField] private LayerMask groundMask;



    

    // Helper variables
    private Vector3 velocity;
    private Vector3 movement;
    private float verticalRotation = 0f;
    private float speedMod;
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




        if (velocity.y < 0 && Input.GetKey(KeyCode.LeftControl)) {
            SetHeight(crouchHeight);
            speedMod = crouchSpeed;
        }
        else{
            SetHeight(baseHeight);
            speedMod = baseSpeed;

            if (isGrounded && velocity.y < 0 && Input.GetKey(KeyCode.LeftShift)) {
                speedMod = sprintSpeed;
            }
        }
        

        Vector3 inputMovement = Input.GetAxis("Vertical") * transform.forward + Input.GetAxis("Horizontal") * transform.right;
        inputMovement *= speedMod * Time.deltaTime;
        
        if(isGrounded && velocity.y < 0){
            velocity.y = staticGravity;
            movement = inputMovement;
        }

        if (isGrounded && velocity.y < 0 && Input.GetButtonDown("Jump")) {
            velocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime + movement);
    }

    private void SetHeight(float height){
        //float radius = characterController.radius;
        characterController.height = height;
        characterController.center = Vector3.up * (.5f * (Mathf.Max((height - 1f),0f) + groundOffset));
        view.transform.localPosition = Vector3.up * (Mathf.Max(height - 1f, 0f) + cameraOffset);
    }
}
