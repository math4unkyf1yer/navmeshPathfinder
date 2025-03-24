using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleFlicker : MonoBehaviour
{
    private Light candleLight;
    private float baseIntensity;

    void Start()
    {
        candleLight = GetComponent<Light>();
        baseIntensity = candleLight.intensity;
        InvokeRepeating("SlowUpdate",0,0.5f);
    }

    void SlowUpdate()
    {
        candleLight.intensity = baseIntensity + Random.Range(-0.2f, 0.2f);
    }
    void Update()
    {
        
    }
}
