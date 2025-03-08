using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeverInteract : MonoBehaviour
{
    public float interactionRadius = 2f; // Adjust this for interaction range
    public Color gizmoColor = Color.green;// Set the gizmo color
    public GameObject player;
    public KeyCode interactKey;// Key to interact
    private PlayerMovement playerMovementScript;
    public TextMeshProUGUI interactText;
    private bool islever = false;

    public Transform[] targetPosition;
    public GameObject[] wall;
    private Vector3[] targetPos;
    private bool isSliding;
    public float slideSpeed = 10f;

    private void Start()
    {
        playerMovementScript = player.GetComponent<PlayerMovement>();
        targetPos = new Vector3[targetPosition.Length];
        for (int i = 0; i < targetPosition.Length; i++)
        {
            targetPos[i] = targetPosition[i].position; // Store each target position
        }
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
        AcessLever();
    }
    void AcessLever()
    {
        if (player != null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < interactionRadius && islever == false)
            {
                interactText.text = "Presse E to move lever";
                // If player presses the interact key and is not hiding
                if (Input.GetKeyDown(interactKey) && islever == false)
                {
                    islever = true;
                    isSliding = true;
                }
            }
            else
            {
                interactText.text = "";
            }
            if (isSliding)
            {
                bool allWallsReached = true;

                for (int i = 0; i < wall.Length; i++)
                {
                    if (i < targetPos.Length) // Ensure index is valid
                    {
                        wall[i].transform.position = Vector3.MoveTowards(wall[i].transform.position, targetPos[i], slideSpeed * Time.deltaTime);

                        if (Vector3.Distance(wall[i].transform.position, targetPos[i]) >= 0.01f)
                        {
                            allWallsReached = false; // If any wall is still moving, keep sliding
                        }
                    }
                }

                if (allWallsReached) // Stop sliding when all walls reach targets
                {
                    isSliding = false;
                }
            }
        }
    }
}
