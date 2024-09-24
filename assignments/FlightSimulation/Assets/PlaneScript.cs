using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class PlaneScript : MonoBehaviour
{
    public GameObject cameraObject;

    public Terrain terrain; 

    //These variables control how the plane moves!
     float forwardSpeed = 30f; // NEW for times Time.deltaTime
    
    float xRotationSpeed = 90f; // NEW for times Time.deltaTime

    float yRotationSpeed = 90f; // NEW for times Time.deltaTime

    private Vector3 startingPosition;
    public float speedIncrease = 5f;

    
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
	AdjustCameraToSeeAll(); // suggested by ChatGPT
        
    }

    // Update is called once per frame
    void Update()
    {
        //Get directional input (up, down, left, right)
        float vAxis = Input.GetAxis("Vertical"); // -1 if down is pressed, 1 if up is pressed, 0 if neither is pressed
        float hAxis = Input.GetAxis("Horizontal"); //-1 if left is pressed, 1 if right is pressed, 0 if neither is pressed

        //float push = Input.GetKey("Spacebar") //if spacebar is pressed

        //Deal with colliding with the terrain
        float terrainHeight = terrain.SampleHeight(transform.position);
            //Debug.Log(terrainHeight);
            //Debug.Log(transform.position.y);
        if (transform.position.y < terrainHeight){
            Vector3 correctedPosition = transform.position;
            correctedPosition.y = terrainHeight; // Ensure plane stays above terrain
            transform.position = correctedPosition;
        }

        //Apply the rotation based on the inputs!
        Vector3 amountToRotate = new Vector3(0,0,0);
        amountToRotate.x = vAxis * xRotationSpeed;  
        amountToRotate.y = hAxis * yRotationSpeed;
        amountToRotate *= Time.deltaTime; //amountToRotate = amountToRotate * Time.deltaTime;
        transform.Rotate(amountToRotate, Space.Self);


        //Make the plane move forward by adding the forward vector to the position.
        transform.position += transform.forward * forwardSpeed * Time.deltaTime;



        //Position the camera!
        Vector3 cameraPosition = transform.position;
        cameraPosition += -transform.forward * 300f;
        cameraPosition += Vector3.up * 150f;  
        cameraObject.transform.position = cameraPosition;

        cameraObject.transform.LookAt(transform.position);
    }
void AdjustCameraToSeeAll()
    {
        // Get terrain size and position
        Vector3 terrainSize = terrain.terrainData.size;
        Vector3 terrainPosition = terrain.transform.position;

        // Adjust the camera to show the entire terrain
        Vector3 cameraPosition = new Vector3(
            terrainPosition.x + terrainSize.x / 2,
            terrainPosition.y + terrainSize.y + 100f,
            terrainPosition.z - terrainSize.z / 2
        );
        
        cameraObject.transform.position = cameraPosition;
    }
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.CompareTag("collectable"))
        {
            Destroy(other.gameObject);
        } 
        else if (other.CompareTag("obstacle")) //crashes with birds
        {
            Crash(); 
        }
        else if (other.CompareTag("gas")) //speed increase if collects gas
        {
            IncreaseSpeed(); 
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("wall"))
        {
            transform.Rotate(0, 180, 0, Space.World); // if player (plane) moves off the map
        }

    }

    private void Crash()
    {
        transform.position = startingPosition;
        forwardSpeed = 0;
        Debug.Log("Crashed into a bird!");
    }
    private void IncreaseSpeed()
    {
        forwardSpeed += speedIncrease;
    }
}