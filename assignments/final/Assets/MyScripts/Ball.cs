using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Ball : MonoBehaviour
{

     //inspiration/references/very helpful w/ occasional logic help from ChatGPT: 
        //https://youtu.be/AWJSa_Pbq7Y
        //https://www.reddit.com/r/Unity3D/comments/6su4ax/2_years_in_development_my_first_unity_game/
        //https://youtu.be/uXIPjMhS86w?list=PLwZ1kAmPIl5vmDziIU1wWGW1lRT7R8C4v
        //https://www.reddit.com/r/Unity3D/comments/4931w2/new_video_of_my_first_ever_unity_game_sociable/
        //https://youtu.be/p7fbpOpG-wk
        //https://discussions.unity.com/t/soccer-game-tutorial/427096
        //https://youtu.be/213RyUE1WiY
        //https://discussions.unity.com/t/soccer-game-with-physics-synchronization-what-are-my-options/862548
        //https://youtu.be/A-nGgl2yR0
        //https://www.quora.com/How-do-I-create-a-football-game-similar-to-FIFA-in-Unity
        //https://www.youtube.com/watch?v=cGwt-1dnHiw&t=122s
        //https://assetstore.unity.com/packages/templates/packs/soccer-project-plus-154230?srsltid=AfmBOoqW9h9tIG3rD4hnhHNPIf6qMwQGbUNqPuAIO5znhA7qD-KTaap4
        //https://youtu.be/Bzw_OOQoH_E
        //https://youtu.be/RkUa8181IzE?list=PLDj0fuWvTM0UQ8JVPVg89QIIcvn06bIoy
        //https://youtu.be/RkUa8181IzE?list=PLDj0fuWvTM0UQ8JVPVg89QIIcvn06bIoy
        //https://docs.unity3d.com/Manual/RootMotion.html
        //https://discussions.unity.com/t/goalkeeper-ai/254404
        //https://stackoverflow.com/questions/49788284/how-to-make-goalkeeper-follow-the-football-in-unity-3d
        //https://discussions.unity.com/t/goal-keeper-ai/449013
        //https://youtu.be/TpQbqRNCgM0?list=PLyBYG1JGBcd009lc1ZfX9ZN5oVUW7AFVy
        //https://youtu.be/UjkSFoLxesw
        //https://youtu.be/pcyiub1hz20
        //https://youtu.be/DX7HyN7oJjE
        //https://youtu.be/f9vRxVgW0WE

    [SerializeField] private Transform transformPlayer;
    private bool playerHasBall;
    private Transform playerBallPosition;
    float speed;
    Vector2 previousLocation;
    Player scriptPlayer;

    public bool HasBeenBlocked { get; set; }

    public bool PlayerHasBall { get => playerHasBall; set => playerHasBall = value;}

    //private Vector3 fieldBoundsMin = new Vector3(-136.2f, 0.175f, -68.3f);
    //private Vector3 fieldBoundsMax = new Vector3(135.93f, 0.175f, 68.7f);



    // Start is called before the first frame update
    void Start()
    {
        GameObject playerRoot = GameObject.FindWithTag("Player");
        if (playerRoot == null)
        {
            Debug.LogError("No GameObject with the 'Player' tag found in the scene.");
            return;
        }

        transformPlayer = playerRoot.transform;

        playerBallPosition = transformPlayer.Find("Geometry").Find("BallLocation");
        scriptPlayer = transformPlayer.GetComponent<Player>();

        if (scriptPlayer == null)
        {
            Debug.LogError("Player script not found on PlayerArmature.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //to get the ball to stick to the player
        if(!PlayerHasBall)
        {
            float distanceToPlayer = Vector3.Distance(transformPlayer.position, transform.position);
            if(distanceToPlayer < 0.5f)
            {
                PlayerHasBall = true;
                scriptPlayer.BallAttachedToPlayer = this;
            }
        }
        else
        {   
            //to get the ball to spin while in motion - dribbling
            Vector2 currentLocation = new Vector2(transform.position.x, transform.position.z);
            speed = Vector2.Distance(currentLocation, previousLocation) / Time.deltaTime;
            transform.position = playerBallPosition.position;
            transform.Rotate(new Vector3(transformPlayer.right.x, 0, transformPlayer.right.z), speed, Space.World);
            previousLocation = currentLocation;
        }

        if(IsOutOfBounds())
        {
            RespawnBall();
        }
        
    }

    private bool IsOutOfBounds()
    {
        return transform.position.x < -123 || transform.position.x > 123 ||
           transform.position.z < -65 || transform.position.z > 65 ||
           transform.position.y < -1;
    }

    private void RespawnBall()
    {
        StartCoroutine(RespawnBallRoutine());
    }

    private IEnumerator RespawnBallRoutine()
    {
        yield return new WaitForSeconds(1f);
        Vector3 respawnPosition = new Vector3(-0.15f, 0.175f, -0.11f);

        
        transform.position = respawnPosition;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        HasBeenBlocked = false;

        scriptPlayer.StartCoroutine(scriptPlayer.DisplayBallRespawnMessage());
    }
}
