using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    //Helpful Link:
    // > https://www.youtube.com/watch?v=o0j7PdU88a4
    // > w/ suggestions from ChatGPT

    float currentTime = 0f;
    float startingTime = 30f;

    [SerializeField] TMP_Text countdownText;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = startingTime;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime;

        if (currentTime < 0) currentTime = 0;

        countdownText.text = Mathf.FloorToInt(currentTime).ToString();
    }
}
