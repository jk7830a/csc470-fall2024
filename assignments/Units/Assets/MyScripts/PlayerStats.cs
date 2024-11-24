using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    // Helpful Links:
    // > https://www.youtube.com/watch?v=yQers6__cLc&t=177s
    // > w/ suggestions from ChatGPT

    private float currentCDs;
    private float maxCDs;

    public UnitBar unitBar;



    // Start is called before the first frame update
    void Start()
    {
        GameObject[] cds = GameObject.FindGameObjectsWithTag("cd");
         if (cds != null && cds.Length > 0)
        {
            maxCDs = cds.Length; // Set maxCDs to the number of CDs in the scene
        }
        else
        {
            Debug.LogError("No objects with the 'CD' tag found in the scene!");
            maxCDs = 0; // Set maxCDs to 0 to prevent errors
        }

        currentCDs = 0;
        unitBar.SetSliderMax(maxCDs);
        unitBar.SetSlider(currentCDs);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCD()
    {
        if (currentCDs < maxCDs)
        {
            currentCDs++;
            Debug.Log($"CDs collected: {currentCDs}/{maxCDs}");
            unitBar.SetSlider(currentCDs);
        }
        else
        {
        Debug.Log("All CDs collected!");
        }
    }
}
