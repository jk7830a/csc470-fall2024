using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SphereScript : MonoBehaviour
{
    public TMP_Text scoreText;
    public Rigidbody rb;

    int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Score: " + score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    
        score++;
        Debug.Log(scoreText.text = "Score: " + score);
    }

    // public void OnCollisionEnter(Collision col)
    // {
    //     Debug.Log("hit the groud!");
    // }
}

