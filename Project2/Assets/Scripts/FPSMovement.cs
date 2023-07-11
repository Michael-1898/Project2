using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class FPSMovement : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float mouseYMax;
    [SerializeField] private float mouseYMin;
    [SerializeField] private float jumpForce;
    [SerializeField] private float castLength;

    private Rigidbody rb;
    private Keyboard kb;
    private Mouse mouse;
    private Vector3 moveDir;

    private float xDir;
    private float yDir;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        kb = Keyboard.current;
        mouse = Mouse.current;

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        moveDir = Vector3.zero;

        if (kb.wKey.isPressed) moveDir += Vector3.forward;
        if (kb.aKey.isPressed) moveDir -= Vector3.right;
        if (kb.sKey.isPressed) moveDir -= Vector3.forward;
        if (kb.dKey.isPressed) moveDir += Vector3.right;

        moveDir = Quaternion.AngleAxis(yDir, Vector3.up) * moveDir;

        if(kb.escapeKey.wasPressedThisFrame)
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
        }

        //How much has the mouse moved since the last frame?
        Vector2 mouseInput = mouse.delta.ReadValue();

        //Note that these are switched!
        xDir -= mouseInput.y * mouseSensitivity;
        yDir += mouseInput.x * mouseSensitivity;

        xDir = Mathf.Clamp(xDir, mouseYMin, mouseYMax);

        transform.rotation = Quaternion.Euler(0, yDir, 0);
        cam.transform.localRotation = Quaternion.Euler(xDir, yDir, 0);
        
        //attempt at fancy camera stuff:
        //------------------------------
        // if(Mathf.Abs(cam.transform.rotation.x) >= 0.05f) {
        //     cam.transform.position = new Vector3(cam.transform.position.x, 2.53f - Mathf.Abs(cam.transform.rotation.x) * 4, cam.transform.position.z);
        // } else {
        //     cam.transform.position = new Vector3(cam.transform.position.x, 2.53f, cam.transform.position.z);
        // }
    }

    private void FixedUpdate() {
        rb.AddForce(moveDir * moveSpeed, ForceMode.VelocityChange);
    }

    private void OnJump(InputValue action) {
        if(IsGrounded()) {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    private bool IsGrounded() {
        return Physics.Raycast(transform.position, -transform.up, castLength);
    }
}