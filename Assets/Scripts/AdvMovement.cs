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
    [SerializeField] private LayerMask checkHeadMask;

    [Header("SFX")]
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip runSound;

    [Header("Smoothing")]
    private float strafeVelocity = 0f;
    [SerializeField] private float strafeMaxSpeed = 1f;
    [SerializeField] private float strafeTime = 1f;
    [SerializeField] private float strafeMultiplier = 1f;


    private enum MoveState {
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

    private float targetStrafeRotation;
    
    private void Temp() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start() {
        Temp();
    }

    private void Update() {
        // Move camera with mouse
        MoveView();

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistanse, groundMask);


        // Set values sepending on a move state (run/walk/..)
        // Crouch
        if (Input.GetKey(KeyCode.LeftControl) && moveState != MoveState.Crouching) {
            SetHeight(crouchHeight);
            speedMod = crouchSpeed;
            movementAudioSource.clip = walkSound;
            moveState = MoveState.Crouching;
        }
        // Sprint
        else if (Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl) && moveState != MoveState.Runing) {
            if(TrySetHeight(baseHeight)){
                speedMod = sprintSpeed;
                movementAudioSource.clip = runSound;
                moveState = MoveState.Runing;
            }
        }
        // Walk
        else if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl) && moveState != MoveState.Walking) {
            if(TrySetHeight(baseHeight)){
                speedMod = baseSpeed;
                movementAudioSource.clip = walkSound;
                moveState = MoveState.Walking;
            }
        }

        // Get movement from input (smoothed)
        Vector3 inputMovement = Vector3.ClampMagnitude(Input.GetAxis("Vertical") * view.forward + Input.GetAxis("Horizontal") * view.right, 1f);
        inputMovement *= speedMod;

        // Rotate camera when strafing
        Quaternion oldquat = view.localRotation;
        Vector3 old = oldquat.eulerAngles;
        view.transform.localRotation = Quaternion.Euler(old.x, old.y, Mathf.SmoothDampAngle(old.z, -Input.GetAxis("Horizontal") * speedMod * strafeMultiplier, ref strafeVelocity, strafeTime, strafeMaxSpeed));

        // Stop/Play audio
        if((inputMovement == Vector3.zero || !isGrounded) && movementAudioSource.isPlaying){
            movementAudioSource.Stop();
        }
        else if (!movementAudioSource.isPlaying){
            movementAudioSource.Play();
        }
        
        // Ground movement
        if(isGrounded && velocity.y < 0){
            velocity.y = staticGravity;
            movement = inputMovement;

            // Add velocity on jump
            if(Input.GetButtonDown("Jump")){
                velocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity);
            }
        }

        // Apply gravity and send to characterController
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime + movement * Time.deltaTime);
    }   

    private void MoveView() { // Rotates the camera according to mouse movements (also clamps vertical rotation)
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation,-90f,90f);

        view.transform.localRotation = Quaternion.Euler(verticalRotation,view.localEulerAngles.y + mouseX,view.localEulerAngles.z);
        //playerGameObject.transform.eulerAngles += new Vector3(0f,mouseX,0f);
    }

    private bool TrySetHeight(float height) { // Checks if there is space above before doing SetHeight()
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

    private void SetHeight(float height){ // Sets the collider and camera to the desired height
        float radius = characterController.radius;
        characterController.height = height;
        characterController.center = Vector3.up * (.5f * (Mathf.Max((height - radius*2),0f) + groundOffset));
        view.transform.localPosition = Vector3.up * (Mathf.Max(height - radius*2, 0f) + cameraOffset * radius);
    }  
}

// public class Tween {
//     private float t;

//     private float clamp;
//     private float transitionTime;


//     private float target_a;
//     private float target_b;

//     public Tween (float a, float b, float transition) {
//         this.t = 0;
//         this.target_a = a;
//         this.target_b = b;
//         this.transitionTime = transition;
//     }

//     public Tween SetTargetA(float a){
//         this.target_a = a;
//         return this;
//     }

//     public Tween SetTargetA(float a, bool reset_t){
//         this.target_a = a;
//         if(reset_t) this.t = 0;
//         return this;
//     }

//     public Tween SetTargetB(float b){
//         this.target_b = b;
//         return this;
//     }

//     public Tween SetTargetB(float b, bool reset_t){
//         this.target_b = b;
//         if(reset_t) this.t = 0;
//         return this;
//     }


//     public float Run() {
//         this.t += 1 / transitionTime * Time.deltaTime;
//         return Mathf.Lerp(this.target_a, this.target_b, this.t);
//     }

// }