using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Code inspired by YouTube with suggestions from ChatGPT (I would ask to explain concepts better or help me debug):
    //https://www.youtube.com/watch?v=L4t2c1_Szdk
    //https://www.youtube.com/watch?v=m9hj9PdO328
    //https://www.youtube.com/watch?v=h2d9Wc3Hhi0&list=PLiyfvmtjWC_V_H-VMGGAZi7n5E0gyhc37&index=1


public class DayNightCycle : MonoBehaviour
{public Camera mainCamera;
    float dayDuration = 90f; 

    //Define the colors for each phase of the day
    private Color dayColor = new Color(1f, 0.87f, 0.58f);  //Yellowish day color
    private Color sunsetColor = new Color(1f, 0.5f, 0.2f); //Red-orange sunset color
    private Color duskColor = new Color(0.1f, 0.3f, 0.5f); //Dusk blue color
    private Color nightColor = Color.black;                //Night color (pitch black)

    private float timer = 0f;
    private float sunsetTime;   //Time to start transitioning to sunset
    private float duskTime;     //Time to start transitioning to dusk
    private float nightTime;    //Time to start transitioning to night

    void Start()
    {
        //Timing for each phase of the day
        sunsetTime = dayDuration * 0.4f;  //40% of the day is daytime
        duskTime = dayDuration * 0.7f;    //70% of the day is sunset
        nightTime = dayDuration * 0.9f;   //90% of the day is dusk, and 10% is night
    }

    void Update()
    {
        
        timer += Time.deltaTime;

        //Day Cycle Loop
        if (timer > dayDuration)
        {
            timer = 0f;
        }

        //Day phase
        if (timer < sunsetTime)
        {
            //Day color to sunset color
            float t = timer / sunsetTime;
            mainCamera.backgroundColor = Color.Lerp(dayColor, sunsetColor, t);
        }
        //Sunset phase
        else if (timer < duskTime)
        {
            //Sunset color to dusk blue
            float t = (timer - sunsetTime) / (duskTime - sunsetTime);
            mainCamera.backgroundColor = Color.Lerp(sunsetColor, duskColor, t);
        }
        //Dusk phase
        else if (timer < nightTime)
        {
            //Dusk blue to night (black)
            float t = (timer - duskTime) / (nightTime - duskTime);
            mainCamera.backgroundColor = Color.Lerp(duskColor, nightColor, t);
        }
        //Night phase
        else
        {
            //Full night, black background
            mainCamera.backgroundColor = nightColor;
        }
    }

    public bool IsNightTime()
    {
        return timer >= nightTime;
    }
}
