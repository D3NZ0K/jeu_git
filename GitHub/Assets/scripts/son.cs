using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using System;
using UnityEngine;

public class son : MonoBehaviour
{
    private AudioSource audioSource;
    public static bool alreadyExist_audio;

    private void Awake()
    {
        if (!alreadyExist_audio)
        {
            alreadyExist_audio = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    
        audioSource = gameObject.AddComponent<AudioSource>();
    }
}

