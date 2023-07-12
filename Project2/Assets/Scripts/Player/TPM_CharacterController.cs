using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TPM_CharacterController : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCheckRadius;
    private bool isGrounded;

    [Header("Input")]
    private Vector3 moveRawInput;
    private Vector2 mouseRawInput;
    private float xRotation;
    private float yRotation;
    private Vector3 moveVelocity;

    [Header("Player Movement")]
    private float yAxisVelocity;
    [SerializeField] private float mouseSensitivity, mouseYMax, mouseYMin;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float gravityMultiplier;

    [Header("Jump")]
    [SerializeField] private float jumpForce;

    [Header("Misc")]
    private Mouse mouse;
    private Camera cam;
    private CharacterController characterController;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        mouse = Mouse.current;

        cam = Camera.main;
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();

        yAxisVelocity += gravity * gravityMultiplier * Time.deltaTime;

        moveVelocity = transform.TransformDirection(moveRawInput) * moveSpeed;
        moveVelocity += new Vector3(0, yAxisVelocity, 0);
        characterController.Move(moveVelocity * Time.deltaTime);
        CursorCheck();

        //animations
        // if(moveRawInput.z > 0.1f) {
        //     SetAnimWalkForward();
        // } else if(moveRawInput.z < -0.1f) {
        //     SetAnimWalkBackward();
        // } else if(moveRawInput.x > 0.1f) {
        //     SetAnimWalkRight();
        // } else if(moveRawInput.x < -0.1f) {
        //     SetAnimWalkLeft();
        // } else {
        //     SetAnimIdle();
        // }
    }

    private void CursorCheck() {
        if (mouse.rightButton.wasPressedThisFrame) {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void OnLook(InputValue value)
    {
        mouseRawInput = value.Get<Vector2>();

        xRotation -= mouseRawInput.y * mouseSensitivity * Time.deltaTime;
        yRotation += mouseRawInput.x * mouseSensitivity * Time.deltaTime;

        xRotation = Mathf.Clamp(xRotation, mouseYMin, mouseYMax);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.localRotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void OnMove(InputValue value)
    {
        moveRawInput = new Vector3(value.Get<Vector2>().x, 0, value.Get<Vector2>().y);
    }

    private void OnJump()
    {
        if(isGrounded) {
            yAxisVelocity = jumpForce;
        }
    }

    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.transform.position, groundCheckRadius, groundMask);
    }

    private void SetAnimWalkForward()
    {
        anim.SetBool("movingForward", true);
        anim.SetBool("movingBackward", false);
        anim.SetBool("movingLeft", false);
        anim.SetBool("movingRight", false);
    }

    private void SetAnimWalkBackward()
    {
        anim.SetBool("movingForward", false);
        anim.SetBool("movingBackward", true);
        anim.SetBool("movingLeft", false);
        anim.SetBool("movingRight", false);
    }

    private void SetAnimWalkLeft()
    {
        anim.SetBool("movingForward", false);
        anim.SetBool("movingBackward", false);
        anim.SetBool("movingLeft", true);
        anim.SetBool("movingRight", false);
    }

    private void SetAnimWalkRight()
    {
        anim.SetBool("movingForward", false);
        anim.SetBool("movingBackward", false);
        anim.SetBool("movingLeft", false);
        anim.SetBool("movingRight", true);
    }

    private void SetAnimIdle()
    {
        anim.SetBool("movingForward", false);
        anim.SetBool("movingBackward", false);
        anim.SetBool("movingLeft", false);
        anim.SetBool("movingRight", false);
    }

    private void SetAnimJump()
    {
        anim.SetBool("movingForward", false);
        anim.SetBool("movingBackward", false);
        anim.SetBool("movingLeft", false);
        anim.SetBool("movingRight", false);
        anim.SetBool("isJumping", true);
    }
}
