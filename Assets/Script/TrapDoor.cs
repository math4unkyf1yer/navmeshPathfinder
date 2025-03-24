using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDoor : MonoBehaviour
{
    public GameObject player;
    private Animator trapDoorAnimation;
    public Collider room3Collider;
    private void Start()
    {
        trapDoorAnimation = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            trapDoorAnimation.SetBool("TrapDoorOpen",true);
            room3Collider.enabled = false;
        }
    }
}
