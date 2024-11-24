using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCDs : MonoBehaviour
{
    //my code from FlightSimulator Assignment
    
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
        transform.localPosition = new Vector3(startPosition.x, newY, startPosition.z );
    }
}
