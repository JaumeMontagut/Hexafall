using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public List<AudioClip> audioClips; 
    private AudioSource audioSource;

    void Awake()
    {
        Managers.Music = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic(string name)
    {
        StopMusic();

        AudioClip audioClip = GetAudioByName(name);

        if (audioClip)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    AudioClip GetAudioByName(string name)
    {
        foreach (AudioClip audioClip in audioClips)
        {
            if (audioClip.name == name)
            {
                return audioClip;
            }
        }
        return null;
    }
}
