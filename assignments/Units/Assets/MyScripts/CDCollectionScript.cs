using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CDCollectionScript : MonoBehaviour
{
    // Helpful Links:
    // > https://www.youtube.com/watch?v=6OT43pvUyfY&t=265s
    // > https://www.youtube.com/watch?v=md7wCkkv_g4
    // > https://www.youtube.com/watch?v=hiA_qRiNgfg
    // > w/ suggestions from ChatGPT
    

// ---CODE---
    private AudioSource currentAudioSource; 
    private Coroutine fadeOutCoroutine;    
    private PlayerStats playerStats;

    
    void OnTriggerEnter(Collider other)
    {
        //check if the player collides with a CD
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Player collided with CD: {gameObject.name}");

            // Notify the PlayerStats script
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.AddCD(); // Increment the CD count and update the slider
                Debug.Log("PlayerStats.AddCD() called!");
            }
            else
            {
                Debug.LogError("Player does not have a PlayerStats component!");
            }

            AudioSource cdAudioSource = GetComponent<AudioSource>(); //gets the AudioSource from CD
            if (cdAudioSource != null)
            {
                Debug.Log($"AudioSource found on CD: {cdAudioSource.clip.name}");

                //if there's already a song playing, it SHOULD stop it with a fade-out effect
                if (currentAudioSource != null && currentAudioSource.isPlaying)
                {
                    if (fadeOutCoroutine != null)
                        StopCoroutine(fadeOutCoroutine);
                    fadeOutCoroutine = StartCoroutine(FadeOut(currentAudioSource, 2f));
                }

                //play the new CD's audio
                if (!cdAudioSource.isPlaying)
                {
                    cdAudioSource.Play();
                }
                currentAudioSource = cdAudioSource;  //updates the current audio source to the new CD

                //destroy the CD after collection
                Destroy(gameObject);   
            }
            else
            {
                Debug.LogWarning("No AudioSource found on CD!");
            }
        }
    }


    IEnumerator FadeOut(AudioSource audioSource, float fadeDuration)
    {
        float startVolume = audioSource.volume;

        //SHOULD fade out the current song
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume; //SHOULD Reset volume for future use
    }
}
