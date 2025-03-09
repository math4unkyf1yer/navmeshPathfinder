using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InvisibilityBook : MonoBehaviour
{
    public PlayerMovement playerMovementScript;
    public int radius = 4;
    public GameObject player;
    public TextMeshProUGUI invisText;

    private void Start()
    {
        playerMovementScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void Update()
    {
        if(player != null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < radius)
            {
                invisText.text = "Press E to learn Invisibility Spell";

                if (Input.GetKeyDown(KeyCode.E))
                {
                    playerMovementScript.isSpellLearned = true;
                    invisText.text = "";
                    playerMovementScript.spellCooldown.gameObject.SetActive(true);
                    playerMovementScript.spellCooldown.value = 5;
                    Destroy(gameObject);
                }
            }
            else
            {
                invisText.text = "";
            }
        }
    }
}
