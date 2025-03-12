using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    public Transform playerpos;

    float xRotation;
    float yRotation;
    public bool cameraMove;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            // Mouse input
            float mouseX = Input.GetAxisRaw("Mouse X") * sensX * Time.deltaTime;
            float mouseY = Input.GetAxisRaw("Mouse Y") * sensY * Time.deltaTime;

            // Controller input (Right Stick)
            float controllerX = Input.GetAxisRaw("Right Stick Horizontal") * sensX * Time.deltaTime;
            float controllerY = Input.GetAxisRaw("Right Stick Vertical") * sensY * Time.deltaTime;


            // Combine inputs (prioritize controller if active)
            float finalX = (Mathf.Abs(controllerX) > 0.1f) ? controllerX : mouseX;
            float finalY = (Mathf.Abs(controllerY) > 0.1f) ? controllerY : mouseY;

            // Apply rotations
            yRotation += finalX;
            xRotation -= finalY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            // Rotate Camera (up/down)
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

            // Rotate Player (left/right)
            playerpos.rotation = Quaternion.Euler(0, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }

    }
}
