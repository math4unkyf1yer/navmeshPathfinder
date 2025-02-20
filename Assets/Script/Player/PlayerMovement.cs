using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("References")]
    public Transform orientation; // Assign an empty GameObject for movement direction

    private Rigidbody rb;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;

    public bool isHiding;
    private Transform unhide;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevents unwanted rotation
    }


    void Update()
    {
        GetInput();        
    }

    void FixedUpdate()
    {
        if(isHiding == false)
        {
            MovePlayer();
        }
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        // Calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        moveDirection.y = 0; // Ensure no vertical movement

        // Apply velocity directly for precise movement
        rb.velocity = moveDirection.normalized * moveSpeed + new Vector3(0, rb.velocity.y, 0);
    }
    public void Hide(Transform hidePos, Transform unHidePos)
    {
        unhide = unHidePos;
        rb.isKinematic = true; // Disables physics but keeps collisions
        rb.useGravity = false;
        transform.position = hidePos.position; // Move to hiding position
        isHiding = true; // Set to hiding state
    }

    public void UnHide()
    {
        isHiding = false;
        rb.isKinematic = false; // Re-enable physics
        rb.useGravity = true;  // Re-enable gravity
        transform.position = unhide.position; // Move back to unhide position
    }
}
