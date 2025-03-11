using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] patrolPoints; // Waypoints for patrol
    public Transform[] patrolPoints1;
    public int whichPatrol = 0;
    private int currentPoint2 = 0;
    private int currentPoint = 0;
    public float detectionRange = 10f; // How far the enemy can see
    public float fieldOfViewAngle = 90f; // Field of view in degrees
    public Transform player;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;

    private NavMeshAgent agent;
    public bool playerInSight = false;

    public AudioSource enemySound;
    public PlayStopAudio audioScript;
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
        if (playerCharacter == null) return;

        CheckForPlayer();

        if (playerInSight)
        {
            ChasePlayer();
        }
        else if (isLurking)
        {
            LurkAtLastKnownPosition();
        }
        else
        {
            Patrol();
        }
    }

    void CheckForPlayer()
    {
        // Reset player detection
        playerInSight = false;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float playerDistance = Vector3.Distance(transform.position, player.position);

        if (playerMovementScript.isHiding || playerMovementScript.isInvisible)
        {
            Lurking(); // Make the enemy search instead of detecting
                       // If the player is within attack range, kill them regardless of invisibility
            if (playerDistance < 2.5f && !playerMovementScript.isHiding)
            {
                PlayerDeath();
                return;
            }
            return;
        }

        // Regular detection logic
        if (playerDistance < detectionRange)
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

        Lurking();
    }

    void Lurking()
    {
        if (!playerInSight && !isLurking && checkIfSawPlayer)
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
            audioScript.StartAudio();
            return;
        }

        if (!enemySound.isPlaying)
        {
            enemySound.clip = enemyChaseSound;
            enemySound.Play();
            audioScript.StopAudio();
        }

        float playerDistance = Vector3.Distance(transform.position, player.position);
        if (playerDistance < 2.5f)
        {
            PlayerDeath();
        }
        else
        {
            agent.speed = 5;
            agent.SetDestination(player.position);
        }
    }

    void LurkAtLastKnownPosition()
    {
        fieldOfViewAngle = 360f; // Increase FOV while searching
        if (!agent.pathPending && agent.remainingDistance < 0.5f) // Ensure the enemy has reached the position
        {
            agent.SetDestination(transform.position); // Stop moving

            currentLurkTime += Time.deltaTime; // Start the lurking timer

            transform.Rotate(0f, 90f * Time.deltaTime, 0f); // Scan the area

            if (currentLurkTime >= lurkingTime)
            {
                isLurking = false;
                checkIfSawPlayer = false;
                fieldOfViewAngle = 90f; // Restore FOV
                Patrol(); // Resume patrol
            }
        }
    }

    void Patrol()
    {
        enemySound.Stop();
        if (!audioScript.levelAudio.isPlaying)
        {
            audioScript.StartAudio();
        }

        if (agent.remainingDistance < 0.5f && !agent.pathPending)
        {
            if(whichPatrol == 0)
            {
                agent.speed = 4;
                currentPoint = (currentPoint + 1) % patrolPoints.Length;
                agent.SetDestination(patrolPoints[currentPoint].position);
            }
            if(whichPatrol == 1)
            {
                currentPoint2 = (currentPoint2 + 1) % patrolPoints1.Length;
                agent.SetDestination(patrolPoints1[currentPoint2].position);
            }
        }
    }

    void PlayerDeath()
    {
        playerIsDEAD = true;
        deadPage.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Destroy(playerCharacter);
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
