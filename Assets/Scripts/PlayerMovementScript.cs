using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField] private  GameObject cameraObj;
    [SerializeField] private Transform groundCheck;
    private CharacterController chControl;
    [SerializeField] private float groundDistanse = 0.4f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float mouseSensivityX = 300f;
    [SerializeField] private float mouseSensivityY = 400f;
    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float sprintSpeed = 20f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float jumpHeight = 3;

    private float verticalRotation = 0f;
    private float speed;
    public Vector3 velocity = new Vector3();
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        chControl = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKey(KeyCode.LeftShift))
        {
            speed = sprintSpeed;
        }
        else
        {
            speed = walkSpeed;
        }
        
        float mouseX = Input.GetAxis("Mouse X") * mouseSensivityX;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensivityY;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation,-90f,90f);
        cameraObj.transform.localRotation = Quaternion.Euler(verticalRotation,0f,0f);
        transform.eulerAngles += new Vector3(0f,mouseX,0f);

        Vector3 move = Input.GetAxis("Vertical") * transform.forward + Input.GetAxis("Horizontal") * transform.right;

        

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistanse, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -0.2f;
        }

        if(isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2F * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        chControl.Move(move * Time.deltaTime * speed + velocity * Time.deltaTime);
    }
}
