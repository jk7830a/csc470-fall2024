using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CDCollectionScript3 : MonoBehaviour
{


    // Helpful Links:
    // > https://www.youtube.com/watch?v=6OT43pvUyfY&t=265s
    // > https://www.youtube.com/watch?v=md7wCkkv_g4
    // > https://www.youtube.com/watch?v=hiA_qRiNgfg
    // > w/ suggestions from ChatGPT



    private AudioManager audioManager;
    private string currentSong;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        currentSong = null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("cd"))
        {
            CDSoundName cd = other.GetComponent<CDSoundName>();
            if (cd != null)
            {
                string newSong = cd.soundName;
                Debug.Log($"CD collected: {other.gameObject.name}, Song: {newSong}");

                if (currentSong != newSong) 
                {
                    StartCoroutine(FadeToNewSong(newSong));
                }

                StartCoroutine(DestroyAfterDelay(other.gameObject, 0.1f));
            }
        }
    }

    IEnumerator DestroyAfterDelay(GameObject cd, float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log($"Destroying CD: {cd.name}");
        Destroy(cd);
    }

    IEnumerator FadeToNewSong(string newSong)
    {
        if (!string.IsNullOrEmpty(currentSong))
        {
            Sound currentSound = audioManager.GetSound(currentSong);
            if (currentSound != null)
            {
                //SHOULD fade out the current song
                while (currentSound.source.volume > 0.01f)
                {
                    currentSound.source.volume -= Time.deltaTime / 1f; 
                    yield return null;
                }
                currentSound.source.Stop();
            }
        }

        //SHOULD fade in the new song
        currentSong = newSong;
        Sound newSound = audioManager.GetSound(newSong);
        if (newSound != null)
        {
            //newSound.source.spatialBlend = 1.0f;
            newSound.source.volume = 0;

            //Debug.Log($"Playing new song: {newSong}");
            
            newSound.source.Play();
            Debug.Log($"Playing audio: {newSound.name}, Is Playing: {newSound.source.isPlaying}");

            Debug.Log($"New song volume before fade: {newSound.source.volume}");
            
            while (newSound.source.volume < newSound.volume)
            {
                newSound.source.volume += Time.deltaTime / 1f; 
            }
        }
    }
}
