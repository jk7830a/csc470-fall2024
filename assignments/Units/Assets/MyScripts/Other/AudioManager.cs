using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{

    // Helpful Links:
    // > https://www.youtube.com/watch?v=6OT43pvUyfY&t=265s
    // > https://www.youtube.com/watch?v=md7wCkkv_g4
    // > https://www.youtube.com/watch?v=hiA_qRiNgfg
    // > w/ suggestions from ChatGPT

    public Sound[] sounds;

    [Range(0f, 1f)]
    public float volume;

    [Range(.1f, 3f)]
    public float pitch;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            Debug.Log($"Added AudioSource to {s.name} on {gameObject.name}");
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.spatialBlend = 1.0f;
            s.source.minDistance = 1f; 
            s.source.maxDistance = 500f;
            s.source.rolloffMode = AudioRolloffMode.Logarithmic;
        }
    }

    void Update()
    {
        foreach (Sound s in sounds)
            {
                if (s.source.isPlaying)
                    {
                        s.source.volume = s.volume;
                        s.source.pitch = s.pitch;
                    }
            }
    }

    public Sound GetSound(string name)
    {
        return Array.Find(sounds, sound => sound.name == name);
    }
}