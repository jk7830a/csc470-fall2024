using UnityEngine.Audio;
using UnityEngine;


    // Helpful Links:
    // > https://www.youtube.com/watch?v=6OT43pvUyfY&t=265s
    // > https://www.youtube.com/watch?v=md7wCkkv_g4
    // > https://www.youtube.com/watch?v=hiA_qRiNgfg
    // > w/ suggestions from ChatGPT



[System.Serializable]
public class Sound{
    
    public string name;
    
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    
    [Range(.1f, 3f)]
    public float pitch;

    [HideInInspector]
    public AudioSource source;
}
