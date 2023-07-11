using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSMovement : MonoBehaviour
{
    Keyboard kb;
    Mouse mouse;

    float xDir;
    float yDir;

    public float moveSpeed;
    public float mouseSensitivity;
    public float mouseYMax;
    public float mouseYMin;

    [SerializeField] Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        kb = Keyboard.current;
        mouse = Mouse.current;

        Vector3 moveDir = Vector3.zero;

        if (kb.wKey.isPressed) moveDir += Vector3.forward;
        if (kb.sKey.isPressed) moveDir -= Vector3.forward;
        if (kb.aKey.isPressed) moveDir -= Vector3.right;
        if (kb.dKey.isPressed) moveDir += Vector3.right;

        moveDir = Quaternion.AngleAxis(yDir, Vector3.up) * moveDir;

        transform.position += moveDir * moveSpeed * Time.deltaTime;

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
        cam.transform.rotation = Quaternion.Euler(xDir, yDir, 0);
        
        //attempt at fancy camera stuff:
        //------------------------------
        // if(Mathf.Abs(cam.transform.rotation.x) >= 0.05f) {
        //     cam.transform.position = new Vector3(cam.transform.position.x, 2.53f - Mathf.Abs(cam.transform.rotation.x) * 4, cam.transform.position.z);
        // } else {
        //     cam.transform.position = new Vector3(cam.transform.position.x, 2.53f, cam.transform.position.z);
        // }
    }
}