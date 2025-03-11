using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayStopAudio : MonoBehaviour
{
    public AudioSource levelAudio;
    

    public void StopAudio()
    {
        levelAudio.Stop();
    }
    public void StartAudio()
    {
        levelAudio.Play();
    }
}
