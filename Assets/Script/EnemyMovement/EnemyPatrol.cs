using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] patrolPoints; // Waypoints for patrol
    private int currentPoint = 0;
    public float detectionRange = 10f; // How far the enemy can see
    public float fieldOfViewAngle = 90f; // Field of view in degrees
    public Transform player;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;

    private NavMeshAgent agent;
    public bool playerInSight = false;

    public AudioSource enemySound;
    public AudioClip enemyChaseSound;

    //player
    public GameObject playerCharacter;
    private PlayerMovement playerMovementScript;
    public GameObject deadPage;
    private bool playerIsDEAD;

    //Lurk 
    private Vector3 lastKnownPosition;
    private bool isLurking = false;
    bool checkIfSawPlayer;
    private float lurkingTime = 3f;
    private float currentLurkTime = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerMovementScript = playerCharacter.GetComponent<PlayerMovement>();
        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[currentPoint].position);
        }
    }

    void Update()
    {
        if(playerMovementScript.isInvisible == true && playerMovementScript != null)
        {
            float playerDistance = Vector3.Distance(transform.position, player.position);
            if (playerDistance < 2.9f)
            {
                playerIsDEAD = true;
                deadPage.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Destroy(playerCharacter);
            }
        }
        CheckForPlayer();
        if (playerInSight)
        {
            ChasePlayer();
        }
        else
        {
            if (isLurking)
            {
                LurkAtLastKnownPosition();
            }
            else
            {
                enemySound.Stop();
                if (agent.remainingDistance < 0.5f && !agent.pathPending)
                {
                    // Move to the next waypoint
                    currentPoint = (currentPoint + 1) % patrolPoints.Length;
                    agent.SetDestination(patrolPoints[currentPoint].position);
                }
            }
        }
    }
    void CheckForPlayer()
    {
        if(playerMovementScript.isInvisible == true)
        {
            playerInSight = false;
            Lurking();
            return;
        }
        if (playerCharacter != null && playerMovementScript.isHiding == false)
        {
            playerInSight = false;
            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            if (Vector3.Distance(transform.position, player.position) < detectionRange)
            {
                float angle = Vector3.Angle(transform.forward, directionToPlayer);
                if (angle < fieldOfViewAngle / 2)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRange))
                    {
                        if (hit.transform.CompareTag("Player"))
                        {
                            playerInSight = true;
                            checkIfSawPlayer = true;
                            isLurking = false; // Stop lurking if the player is found
                        }
                    }
                }
            }
        }

        Lurking();
    }
    void Lurking()
    {
        // If the player was in sight but now isn't, start lurking
        if (!playerInSight && !isLurking && checkIfSawPlayer == true)
        {
            lastKnownPosition = player.position;
            isLurking = true;
            currentLurkTime = 0f; // Reset the lurking timer
        }
    }
 
    void ChasePlayer()
    {
        if (playerIsDEAD)
        {
            enemySound.Stop();
            return;
        }
        if (agent != null && playerCharacter !=null)
        {
            if (!enemySound.isPlaying)
            {
                enemySound.clip = enemyChaseSound;
                enemySound.Play();
            }
            float playerDistance = Vector3.Distance(transform.position, player.position);
            if (playerDistance < 2.9f)
            {
                playerIsDEAD = true;
                deadPage.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Destroy(playerCharacter);
            }
            else
            {
                agent.SetDestination(player.position);
            }
        }
    }
    void LurkAtLastKnownPosition()
    {
        // Change the enemy's field of view to 360 for searching the area
        fieldOfViewAngle = 360f;

        // Move towards the last known position of the player
        agent.SetDestination(lastKnownPosition);

        // Check if the enemy is at the last known position
        if (Vector3.Distance(transform.position, lastKnownPosition) < 1f)
        {
            // Start rotating to scan the area
            transform.Rotate(0f, 90f * Time.deltaTime, 0f); // Rotate the enemy around its axis

            // Increase the lurking time
            currentLurkTime += Time.deltaTime;

            if (currentLurkTime >= lurkingTime)
            {
                // After the lurking time, stop looking and return to patrolling
                isLurking = false;
                checkIfSawPlayer = false;
                fieldOfViewAngle = 90f; // Restore original field of view
                agent.SetDestination(patrolPoints[currentPoint].position); // Start patrolling again
            }
        }
    }

        void OnDrawGizmos()
    {
        if (player == null) return;

        // Draw detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Draw field of view lines
        Vector3 forward = transform.forward;
        Vector3 leftLimit = Quaternion.Euler(0, -fieldOfViewAngle / 2, 0) * forward;
        Vector3 rightLimit = Quaternion.Euler(0, fieldOfViewAngle / 2, 0) * forward;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + leftLimit * detectionRange);
        Gizmos.DrawLine(transform.position, transform.position + rightLimit * detectionRange);

        // Draw line to the player if visible
        if (playerInSight)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, player.position);
        }

        // Draw last known position (for debugging purposes)
        if (isLurking)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, lastKnownPosition);
        }
    }

}
