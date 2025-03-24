using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletChase : MonoBehaviour
{
    private GameObject player;
    private Whisp enemyScript;
    public int speed = 6;

    private void Start()
    {
        player = GameObject.Find("Player");
        enemyScript = GameObject.Find("Whisp").GetComponent<Whisp>();
        StartCoroutine(DestroyObject());
    }

    private void Update()
    {
        if(player != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == player)
        {
            enemyScript.PlayerDeath();
        }
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }
}
