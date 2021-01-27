﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        Managers.Music = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void StartMusic()
    {
        audioSource.Play();
    }
}
