using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;
    public float shakeAmount = 0.1f;
    public EnemyPatrol enemyScript;
    private void Start()
    {
        enemyScript = GameObject.Find("Enemy").GetComponent<EnemyPatrol>();
    }
    // Update is called once per frame
    void Update()
    {
        if(cameraPosition != null)
        {
            transform.position = cameraPosition.position;
            if (enemyScript.playerInSight)
            {
                transform.position = cameraPosition.position + Random.insideUnitSphere * shakeAmount;
            }
        }
    }
}
