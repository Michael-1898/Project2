using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TPM_CharacterController : MonoBehaviour {
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

    [Header("Knockback")]
    private bool isKnockBackApplied;
    private float knockbackSpeed;
    private float kbTimer;
    [SerializeField] float kbDuration;

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
        anim = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();

        yAxisVelocity += gravity * gravityMultiplier * Time.deltaTime;

        if(!isKnockBackApplied) {
            moveVelocity = transform.TransformDirection(moveRawInput) * moveSpeed;
            moveVelocity += new Vector3(0, yAxisVelocity, 0);
        } else {
            kbTimer += Time.deltaTime;
            if(kbTimer >= kbDuration) {
                isKnockBackApplied = false;
                kbTimer = 0;
            }
        }
        characterController.Move(moveVelocity * Time.deltaTime);
        CursorCheck();
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

        anim.SetBool("WalkFront", moveRawInput.z > 0);
        anim.SetBool("WalkBack", moveRawInput.z < 0);
        anim.SetBool("WalkLeft", moveRawInput.x < 0);
        anim.SetBool("WalkRight", moveRawInput.x > 0);
    }

    private void OnJump()
    {
        if(isGrounded) {
            yAxisVelocity = jumpForce;
            anim.SetTrigger("Jump");
        }
    }

    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.transform.position, groundCheckRadius, groundMask);
    }

    public void TakeKnockback(float kbSpeed, Vector3 kbDir)
    {
        moveVelocity = new Vector3(kbDir.x, kbDir.y, kbDir.z) * kbSpeed;
        isKnockBackApplied = true;
    }
}
