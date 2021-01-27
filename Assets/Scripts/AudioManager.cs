using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEvents;

public class AudioManager : MonoBehaviour
{
    public List<AudioClip> audioClips;

    private void Awake()
    {
        Managers.Audio = this;
    }

    public void PlayAudio( object info)
    {
        string name = (string)info;

        foreach (AudioClip audioClip in audioClips)
        {
            if (audioClip.name.Equals(name))
            {
                StartCoroutine(PlayAudio(audioClip));
                return;
            }
        }
    }

    IEnumerator PlayAudio( AudioClip clip)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();

        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            if (!audioSource.isPlaying)
            {
                Destroy(audioSource);
                yield break;
            }
        }
    }

}
