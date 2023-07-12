using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TPM_CharacterController : MonoBehaviour
{
    private Mouse mouse;
    private Camera cam;
    private CharacterController characterController;
    [SerializeField] Animator anim;

    //Grounded Variables
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundMask;
    private bool isGrounded;

    //Player Input Variables
    private Vector3 moveRawInput;
    private Vector2 mouseRawInput;
    private float xRotation;
    private float yRotation;
    private Vector3 moveVelocity;

    //movement variables
    private float yAxisVelocity;
    [SerializeField] float moveSpeed;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float gravityMultiplier;
    [SerializeField] float mouseYMax;
    [SerializeField] float mouseYMin;
    [SerializeField] float mouseSensitivity;

    //jumping variables
    [SerializeField] float jumpSpeed;
    private bool isJumping;
    [SerializeField] float jumpDuration;
    private float jumpTimer;

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

        if(isGrounded) {
            yAxisVelocity = -2f;
        } else {
            yAxisVelocity += gravity * gravityMultiplier * Time.deltaTime;
        }
        
        if(isJumping) {
            Jump();
        }
        if(anim.GetBool("isJumping") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f) {
            anim.SetBool("isJumping", false);
        }

        moveVelocity = transform.TransformDirection(moveRawInput) * moveSpeed;
        moveVelocity += new Vector3(0, yAxisVelocity, 0);
        characterController.Move(moveVelocity * Time.deltaTime);

        //animations
        if(moveRawInput.z > 0.1f) {
            SetAnimWalkForward();
        } else if(moveRawInput.z < -0.1f) {
            SetAnimWalkBackward();
        } else if(moveRawInput.x > 0.1f) {
            SetAnimWalkRight();
        } else if(moveRawInput.x < -0.1f) {
            SetAnimWalkLeft();
        } else {
            SetAnimIdle();
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

    private void OnJump(InputValue value)
    {
        if(isGrounded) {
            isJumping = true;
            SetAnimJump();
        }
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.transform.position, groundCheckRadius, groundMask);
    }

    void Jump()
    {
        yAxisVelocity = jumpSpeed;
        jumpTimer += Time.deltaTime;
        if(jumpTimer >= jumpDuration) {
            jumpTimer = 0;
            isJumping = false;
        }
    }

    void OnDrawGizmosSelected()
    {

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
