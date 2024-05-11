using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundEffect : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundFX(AudioClip soundFX, float volume = 1, bool randomize = true, float pitchRandom = 0.1f)
    {
        audioSource.PlayOneShot(soundFX, volume);
        audioSource.pitch = 1;

        if (randomize)
        {
            audioSource.pitch += Random.Range(-pitchRandom, pitchRandom);
        }
    }
}