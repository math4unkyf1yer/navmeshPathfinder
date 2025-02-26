using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("References")]
    public Transform orientation; // Assign an empty GameObject for movement direction

    private Rigidbody rb;
    private float horizontalInput;
    private float verticalInput;
     public float runSpeedMultiplier = 1.7f; // Speed multiplier when running
    public float maxStamina = 100f; // Max stamina value
    public float staminaDrainRate = 17f; // How much stamina is drained per second while running
    public float staminaRegenRate = 3.5f; // How much stamina regenerates per second when not running
    public float staminaCooldownTimer = 3f; // Cooldown time before stamina can regenerate
    private float currentStamina; // Current stamina value
    private Vector3 moveDirection;

    public Slider staminaSlider;

    public bool isHiding;
    public bool running;
    private Transform unhide;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevents unwanted rotation
        currentStamina = maxStamina; // Initialize stamina
        staminaCooldownTimer = 0f; // Initialize the cooldown timer
        staminaSlider.maxValue = maxStamina;
    }


    void Update()
    {
        Debug.Log(currentStamina);
        GetInput();
        UpdateStaminaSlider();
        HandleStamina();
        
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

        running = Input.GetKey(KeyCode.LeftShift) && currentStamina > 0f;
    }

    private void MovePlayer()
    {
        // Calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        moveDirection.y = 0; // Ensure no vertical movement

        float currentSpeed = running ? moveSpeed * runSpeedMultiplier : moveSpeed;
        // Apply velocity directly for precise movement
        rb.velocity = moveDirection.normalized * currentSpeed + new Vector3(0, rb.velocity.y, 0);
    }

    private void HandleStamina()
    {
        // If stamina is above 0, drain stamina when running
        if (running)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            if (currentStamina < 0) 
            {
                currentStamina = 0;
                staminaCooldownTimer = 3; // Start cooldown when stamina reaches zero
                moveSpeed = 2;
            }
        }
        else
        {
            // If the cooldown timer is running, do not regenerate stamina
            if (staminaCooldownTimer > 0f)
            {
                staminaCooldownTimer -= Time.deltaTime;
            }
            else
            {
                if (moveSpeed != 5) moveSpeed = 5;
                // Regenerate stamina when not running and cooldown has passed
                currentStamina += staminaRegenRate * Time.deltaTime;
                if (currentStamina > maxStamina) currentStamina = maxStamina;
            }
        }
    }
    private void UpdateStaminaSlider()
    {
        if (staminaSlider != null)
        {
            // Update the stamina slider to reflect current stamina value
            staminaSlider.value = currentStamina;
        }
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
