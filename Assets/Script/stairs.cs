using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sTAIRS : MonoBehaviour
{
    public GameObject player;
    public int sceneChange = 0;

    private void Start()
    {
      //  player = GameObject.Find("Player").GetComponent<GameObject>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == player)
        {
            SceneManager.LoadScene(sceneChange);
        }
    }
}
