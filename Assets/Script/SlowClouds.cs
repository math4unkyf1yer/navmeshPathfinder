using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowClouds : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime * 10f);

        if (transform.position.x > 1940f)
        {
            transform.Translate(new Vector3(-3350, 0, 0));
        }
    }
}
