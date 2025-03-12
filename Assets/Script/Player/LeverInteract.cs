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
    private EnemyPatrol enemyScript;
    public TextMeshProUGUI interactText;
    private bool islever = false;

    public Transform[] targetPosition;
    public GameObject[] wall;
    private Vector3[] targetPos;
    private bool isSliding;
    public float slideSpeed = 10f;
    public int changePatrol;
    public AudioSource[] audioSources;
    private Animator leverAnimation;


    private void Start()
    {
        leverAnimation = gameObject.GetComponent<Animator>();
        playerMovementScript = player.GetComponent<PlayerMovement>();
        if(GameObject.Find("Enemy") != null)
        {
            enemyScript = GameObject.Find("Enemy").GetComponent<EnemyPatrol>();
        }
        targetPos = new Vector3[targetPosition.Length];
        for (int i = 0; i < targetPosition.Length; i++)
        {
            targetPos[i] = targetPosition[i].position; // Store each target position
        }

        audioSources = GetComponents<AudioSource>();
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

    public static void PlayClipAtPoint(AudioClip clip, Vector3 position, [UnityEngine.Internal.DefaultValue("1.0F")] float volume)
    {
        GameObject gameObject = new GameObject("One shot audio");
        gameObject.transform.position = position;
        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        audioSource.clip = clip;
        audioSource.spatialBlend = 1f;
        audioSource.volume = volume;
        audioSource.Play();
        Object.Destroy(gameObject, clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
    }

    void AcessLever()
    {
        if (player != null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < interactionRadius && islever == false)
            {
                interactText.text = "Press E to move lever";
                // If player presses the interact key and is not hiding
                if (Input.GetKeyDown(interactKey) && islever == false)
                {
                    if(enemyScript != null)
                    {
                        leverAnimation.SetBool("Pull", true);
                        enemyScript.whichPatrol = changePatrol;
                        audioSources[0].Play();
                        PlayClipAtPoint(audioSources[1].clip, wall[0].transform.position, 1f);
                    }
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
