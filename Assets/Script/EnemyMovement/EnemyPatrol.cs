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
    private bool playerInSight = false;

    //player
    public GameObject playerCharacter;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[currentPoint].position);
        }
    }

    void Update()
    {
        CheckForPlayer();
        if (playerInSight)
        {
            ChasePlayer();
            Debug.Log("Found player");
        }
        else
        {
            if (agent.remainingDistance < 0.5f && !agent.pathPending)
            {
                // Move to the next waypoint
                currentPoint = (currentPoint + 1) % patrolPoints.Length;
                agent.SetDestination(patrolPoints[currentPoint].position);
            }
        }
    }
    void CheckForPlayer()
    {
        if(playerCharacter != null)
        {
            playerInSight = false;

            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            // Check if the player is within detection range
            if (Vector3.Distance(transform.position, player.position) < detectionRange)
            {
                // Check if player is within the field of view angle
                float angle = Vector3.Angle(transform.forward, directionToPlayer);
                if (angle < fieldOfViewAngle / 2)
                {
                    // Raycast to check if there is an obstacle in the way
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRange))
                    {
                        if (hit.transform.CompareTag("Player")) // Make sure the player has the "Player" tag
                        {
                            playerInSight = true;
                        }
                        else
                        {
                            playerInSight = false; // Something is blocking the view
                        }
                    }
                }
            }
        }
        else
        {
            playerInSight = false;
        }
    }

    void ChasePlayer()
    {
        if (agent != null && playerCharacter !=null)
        {
            float playerDistance = Vector3.Distance(transform.position, player.position);
            Debug.Log(playerDistance);
            if (playerDistance < 2.9f)
            {
                Destroy(playerCharacter);
            }
            else
            {
                agent.SetDestination(player.position);
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
    }

}
