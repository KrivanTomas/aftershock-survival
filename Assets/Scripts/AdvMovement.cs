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
    [SerializeField] private AudioSource movementAudioSource;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform view;[Header("Settings")]

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
    [SerializeField] private LayerMask checkHeadMask;

    [Header("Settings")]
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip runSound;



    private enum MoveState{
        Walking,
        Runing,
        Crouching,
        NotSet
    }



    //
    private float stamina;
    private MoveState moveState = MoveState.NotSet;

    // Helper variables
    private Vector3 velocity = Vector3.zero;
    private Vector3 movement;
    private float verticalRotation = 0f;
    private float speedMod;
    private bool isGrounded = false;
    private bool shut = false;
    
    private void Temp() {
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void Start() {
        Temp();
    }

    private void Update() {

        if(shut) {
            if (movementAudioSource.isPlaying){
                movementAudioSource.Stop();
            }
            return;
        }

        MoveView();

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistanse, groundMask);

        if (Input.GetKey(KeyCode.LeftControl) && moveState != MoveState.Crouching) {
            SetHeight(crouchHeight);
            speedMod = crouchSpeed;
            movementAudioSource.clip = walkSound;
            moveState = MoveState.Crouching;
        }
        else if (Input.GetKey(KeyCode.LeftShift) && moveState != MoveState.Runing) {
            if(TrySetHeight(baseHeight)){
                speedMod = sprintSpeed;
                movementAudioSource.clip = runSound;
                moveState = MoveState.Runing;
            }
        }
        else if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl) && moveState != MoveState.Walking) {
            if(TrySetHeight(baseHeight)){
                speedMod = baseSpeed;
                movementAudioSource.clip = walkSound;
                moveState = MoveState.Walking;
            }
        }



        Vector3 inputMovement = Vector3.ClampMagnitude(Input.GetAxis("Vertical") * transform.forward + Input.GetAxis("Horizontal") * transform.right, 1f);
        inputMovement *= speedMod;

        if((inputMovement == Vector3.zero || !isGrounded) && movementAudioSource.isPlaying){
            movementAudioSource.Stop();
        }
        else if (!movementAudioSource.isPlaying){
            movementAudioSource.Play();
        }
        

        if(isGrounded && velocity.y < 0){
            velocity.y = staticGravity;
            movement = inputMovement;
            if(Input.GetButtonDown("Jump")){
                velocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity);
            }
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime + movement * Time.deltaTime);
    }   

    private void MoveView() {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation,-90f,90f);

        view.transform.localRotation = Quaternion.Euler(verticalRotation,0f,0f);
        playerGameObject.transform.eulerAngles += new Vector3(0f,mouseX,0f);
    }

    private bool TrySetHeight(float height) {
        float radius = characterController.radius;
        bool isBlocked = Physics.CheckSphere(
            playerGameObject.transform.TransformPoint(Vector3.up * Mathf.Max(height - radius*2, 0f)),
            radius, checkHeadMask);

        if(!isBlocked){
            SetHeight(height);
            return true;
        }
        return false;
    }

    private void SetHeight(float height){
        float radius = characterController.radius;
        characterController.height = height;
        characterController.center = Vector3.up * (.5f * (Mathf.Max((height - radius*2),0f) + groundOffset));
        view.transform.localPosition = Vector3.up * (Mathf.Max(height - radius*2, 0f) + cameraOffset * radius);
    }   

    public void Shut(bool doShut){
        shut = doShut;
        if(shut){
            Cursor.lockState = CursorLockMode.Confined;
        }
        else{
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
