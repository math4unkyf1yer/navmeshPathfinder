using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interact : MonoBehaviour
{
    public float interactionRadius = 2f; // Adjust this for interaction range
    public Color gizmoColor = Color.green;// Set the gizmo color
    public GameObject player;
    public KeyCode interactKey;// Key to interact
    private PlayerMovement playerMovementScript;
    public Transform hidePos;
    public Transform unHidePos;
    public TextMeshProUGUI interactText;

    private void Start()
    {
        playerMovementScript = player.GetComponent<PlayerMovement>();
    }
    void OnDrawGizmosSelected()
    {
        // Set the color
        Gizmos.color = gizmoColor;

        // Draw a wire sphere around the player
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
    private void Update()
    {
        if(player != null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < interactionRadius)
            {
                if (playerMovementScript.isHiding == false)
                    interactText.text = "Presse E to hide";
                else
                    interactText.text = "Presse E to unhide";
                // If player presses the interact key and is not hiding
                if (Input.GetKeyDown(interactKey) && !playerMovementScript.isHiding)
                {
                    // Trigger hide functionality
                    playerMovementScript.Hide(hidePos, unHidePos);
                }
                // If player is hiding, allow unhiding when pressing interact key
                else if (Input.GetKeyDown(interactKey) && playerMovementScript.isHiding)
                {
                    // Trigger unhide functionality
                    playerMovementScript.UnHide();
                }
            }
            else
            {
                if(interactText.text != "")
                {
                    interactText.text = "";
                }
            }
        }
    }
}
