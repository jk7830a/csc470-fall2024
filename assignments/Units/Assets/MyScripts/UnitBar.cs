using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitBar : MonoBehaviour
{
    // Helpful Links:
    // > https://www.youtube.com/watch?v=yQers6__cLc&t=177s
    // > w/ suggestions from ChatGPT
    
    public Slider unitSlider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSlider(float amount)
    {
        Debug.Log($"Updating slider to: {amount}");
        unitSlider.value = amount;
    }

    public void SetSliderMax(float amount)
    {
        unitSlider.maxValue = amount;
        unitSlider.value = 0;;
    }
}
