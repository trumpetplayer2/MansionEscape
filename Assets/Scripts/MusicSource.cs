using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSource : MonoBehaviour
{
    public AudioClip Music;
    public AudioSource musicSource;
    void Update()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }
}
