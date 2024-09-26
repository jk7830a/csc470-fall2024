using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsScript : MonoBehaviour
{
    // code from - https://www.youtube.com/watch?v=C7sPsksH4JM w/ suggestions from ChatGPT
    float amp = 0.9f;
    float freq = 2f;

    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * freq ) * amp;
        transform.localPosition = new Vector3(startPosition.x, newY, startPosition.y );
    }
}
