using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaneScript : MonoBehaviour
{
    public GameObject cameraObject;
    public Terrain terrain;

    public TMP_Text scoreText;
    int score = 0; 

    public TMP_Text loseMessageText; 

    float forwardSpeed = 0.3f;
    float rotationSpeed = 45f;

    //for speed boost when collecting a bolt
    float speedIncrease = 0.05f;
    float maxSpeed = 0.5f;

    bool gameStarted = false;
    Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
       startPosition = transform.position;
       scoreText.text = "Score: 0";
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStarted)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                gameStarted = true;
            }
            return;
        }
        
        //make the plane move forward
        transform.position += transform.forward * forwardSpeed;

        float verticalInput = 0f;
        float horizontalInput = 0f; 

        //plane controls; helpful link - https://discussions.unity.com/t/smoother-turning-for-my-aircraft/738453
        if(Input.GetKey(KeyCode.UpArrow))
        {
            //transform.Rotate(-xRotationSpeed * Time.deltaTime, 0, 0);
            
            verticalInput = 1f;
        }

        if(Input.GetKey(KeyCode.DownArrow))
        {
            //transform.Rotate(xRotationSpeed * Time.deltaTime, 0, 0);

            verticalInput = -1f;
        }

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            //transform.Rotate(0, -yRotationSpeed * Time.deltaTime, 0);
            horizontalInput = -1f;
        }

        if(Input.GetKey(KeyCode.RightArrow))
        {
            //transform.Rotate(0, yRotationSpeed * Time.deltaTime, 0);
            horizontalInput = 1f;
        }

        transform.Rotate(verticalInput * rotationSpeed* Time.deltaTime, horizontalInput * rotationSpeed * Time.deltaTime, 0);

        //terrain colliding
        float terrainHeight = terrain.SampleHeight(transform.position);
        if (transform.position.y < terrainHeight)
        {
            GameOver(); 
        }


        //Position the camera
        Vector3 cameraPosition = transform.position + transform.up * 40f - transform.forward * 42f;
        //cameraPosition += -transform.forward * 20f;
        //cameraPosition += Vector3.up * 18f;
        cameraObject.transform.position = cameraPosition;
        cameraObject.transform.LookAt(transform.position);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("collectable"))
        {   
            Debug.Log("Collectable");

            score ++;
            scoreText.text = "Score:" + score;

            Destroy(other.gameObject);
        }
        else if(other.CompareTag("speed"))
        {
            Debug.Log("Speed Increase");
            IncreaseSpeed();
            Destroy(other.gameObject);
        }
        else if(other.CompareTag("wall"))
        {
            Debug.Log("Hit Wall");
            transform.Rotate(0, 180, 0, Space.World);
        }
        else
        {
            GameOver();
        }
    }

    void IncreaseSpeed()
    {
        if(forwardSpeed < maxSpeed)
        {
            forwardSpeed += speedIncrease;
        }
    }

    void GameOver()
    {
         Debug.Log("Game Over!");
    
        if (loseMessageText != null) // Check if loseMessageText is not null
        {
            scoreText.text = "You Lose!";
        }
    
        Destroy(gameObject);
    }
}
