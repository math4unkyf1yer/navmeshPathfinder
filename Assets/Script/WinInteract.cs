using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinInteract : MonoBehaviour
{
    public float interactionRadius = 2f;
    public GameObject player;
    public GameObject winPage;
    public GameObject level;
    public AudioSource EnemySound;
    public AudioSource mainSound;

    void OnDrawGizmosSelected()
    {
        // Set the color
        Gizmos.color = Color.blue;

        // Draw a wire sphere around the player
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }

    private void Update()
    {
        if(player != null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < interactionRadius)
            {
                EnemySound.Stop();
                mainSound.Stop();
                winPage.SetActive(true);
                level.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}
