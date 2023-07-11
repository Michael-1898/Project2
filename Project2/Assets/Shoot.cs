using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayerMask;

    private Mouse mouse;
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mouse = Mouse.current;
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // If the left mouse button is pressed

        if (mouse.leftButton.wasPressedThisFrame)
        {
            // Create a ray from the camera to the mouse position
            Ray ray = mainCamera.ScreenPointToRay(mouse.position.ReadValue());

            // Create a RaycastHit variable to store information about what was hit by the ray
            RaycastHit hit;

            // If the ray hit something
            if (Physics.Raycast(ray, out hit, enemyLayerMask))
            {
                Debug.Log("Hit " + hit.collider.gameObject.name);
                Destroy(hit.collider.gameObject);
            }
        }
    }
}
