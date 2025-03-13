using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

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

    private Slider staminaSlider;
    public Slider spellCooldown;

    public bool isHiding;
    public bool running;
    public bool isSpellLearned;
    private Transform unhide;
    private bool isOnCooldown = false;
    private float cooldownDuration = 5f;
    private float abilityCooldown = 30f;
    public bool isInvisible;
    private Vignette vignette;
    public PostProcessVolume postProcessVolume;
    public AudioSource soundList;
    public AudioSource walking;
    public AudioSource runing;

    void Start()
    {
        staminaSlider = GameObject.Find("StaminaSlider").GetComponent<Slider>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevents unwanted rotation
        currentStamina = maxStamina; // Initialize stamina
        staminaCooldownTimer = 0f; // Initialize the cooldown timer
        staminaSlider.maxValue = maxStamina;
        if (postProcessVolume != null)
        {
            postProcessVolume.profile.TryGetSettings(out vignette);
        }

        if (vignette != null)
        {
            vignette.intensity.value = 0f;
        }

        soundList = GetComponent<AudioSource>();
    
    }


    void Update()
    {
        GetInput();
        // Only proceed if the player is moving and not hiding
        if ((horizontalInput != 0 || verticalInput != 0) && !isHiding)
        {
            if (running) // Running state
            {
                // If running sound isn't already playing, play running sound
                if (!runing.isPlaying)
                {
                    walking.enabled = false; // Stop walking sound if running
                    runing.enabled = true; // Play running sound
                }
            }
            else // Walking state (when not running)
            {
                // If running sound is playing, stop it
                if (runing.isPlaying)
                {
                    runing.enabled = false;
                }

                // Only play walking sound if it's not already playing
                if (!walking.isPlaying)
                {
                    walking.enabled = true;
                }
            }
        }
        else // If not moving
        {
            walking.enabled = false; // Stop walking sound
            runing.enabled = false; // Stop running sound
        }
        if (Input.GetKeyDown(KeyCode.Q) && isSpellLearned && !isOnCooldown || Input.GetKeyDown(KeyCode.JoystickButton3) && isSpellLearned && !isOnCooldown)
        {
            soundList.Play();
            Debug.Log("player is invisible");
            ActivateInvisibility();
        }
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

        running = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.JoystickButton8)) && currentStamina > 0f;
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        moveDirection.y = 0; // Prevent vertical movement
       
        float currentSpeed = running ? moveSpeed * runSpeedMultiplier : moveSpeed;

        // Apply movement with controller sensitivity
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
    void ActivateInvisibility()
    {
        if (!isInvisible)
        {

            isOnCooldown = true;
            isInvisible = true;

            if (spellCooldown != null)
            {
                spellCooldown.gameObject.SetActive(true);
                spellCooldown.maxValue = cooldownDuration;
                spellCooldown.value = cooldownDuration;
            }

            if (vignette != null)
            {
                StartCoroutine(FadeVignette(0f, 0.5f, 0.5f));
            }

            StartCoroutine(InvisibilityCooldown());
        }
    }

    IEnumerator InvisibilityCooldown()
    {
        float timer = cooldownDuration;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            if (spellCooldown != null)
            {
                spellCooldown.value = timer;
            }
            yield return null;
        }

        if (vignette != null)
        {
            StartCoroutine(FadeVignette(0.5f, 0f, 0.5f));
        }

        isInvisible = false;
        StartCoroutine(AbilityCooldownn());
    }
    IEnumerator AbilityCooldownn()
    {
        float timer = abilityCooldown;
        float sliderTimer = 0f;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            sliderTimer = (30f - timer) / 30f * 5f;

            if (spellCooldown != null)
            {
                spellCooldown.value = sliderTimer;
            }

            yield return null;
        }

        isOnCooldown = false;

    }
    IEnumerator FadeVignette(float startValue, float endValue, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(startValue, endValue, elapsed / duration);
            yield return null;
        }

        vignette.intensity.value = endValue;
    }
}
