using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
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

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI goalText;

    [SerializeField] private TextMeshProUGUI instructionText;

    private float goalTextColorAlpha;

    private StarterAssetsInputs starterAssetsInputs;
    private Animator animator;
    private Ball ballAttachedToPlayer;
    private float timeShot = -1f;
    public const int ANIMATION_LAYER_SHOOT = 1;

    private int myScore;
    private int opponentScore;
    private int successfulGoals;

    private AudioSource soundKick;
    private AudioSource soundCheers;

    public Ball BallAttachedToPlayer{get => ballAttachedToPlayer; set => ballAttachedToPlayer = value; }

    // Start is called before the first frame update
    void Start()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();

        soundKick = GameObject.Find("Sound/kick").GetComponent<AudioSource>();
        soundCheers = GameObject.Find("Sound/cheering").GetComponent<AudioSource>();

        successfulGoals = 0;

        StartCoroutine(ShowInstructions());
    }

    // Update is called once per frame
    void Update()
    {
        if(starterAssetsInputs.shoot)
        {
            starterAssetsInputs.shoot = false;
            timeShot = Time.time;
            animator.Play("Shoot", ANIMATION_LAYER_SHOOT, 0f);
            animator.SetLayerWeight(ANIMATION_LAYER_SHOOT, 1);
            Debug.Log("Shoot!");
        }
        
        if(timeShot > 0)
        {
            //to kick ball
            if(ballAttachedToPlayer != null && Time.time - timeShot > 0.2f)
            {
                soundKick.Play();
                ballAttachedToPlayer.PlayerHasBall = false;

                Rigidbody rb = ballAttachedToPlayer.transform.gameObject.GetComponent<Rigidbody>();
                Vector3 shootDirection = transform.forward;
                shootDirection.y += 0.45f;
                rb.AddForce(shootDirection * 15f, ForceMode.Impulse);

                ballAttachedToPlayer = null;
            } 

            if(Time.time - timeShot > 0.5f)
            {
                timeShot = -1f;
            }
        }
        else{
            animator.SetLayerWeight(ANIMATION_LAYER_SHOOT, Mathf.Lerp(animator.GetLayerWeight(ANIMATION_LAYER_SHOOT), 0f, Time.deltaTime * 10f));
        }

        if(goalTextColorAlpha > 0)
        {
            goalTextColorAlpha -= Time.deltaTime;
            goalText.alpha = goalTextColorAlpha;
            goalText.fontSize = 200 - (goalTextColorAlpha * 1-0);
        }
    }

    public void IncreaseMyScore()
    {
        myScore++;
        successfulGoals++;
        UpdateScore();

        if (successfulGoals >= 3) 
        {
            DisplayThreeShotsMessage();
        }
    
    }

    public void IncreaseOpponentScore()
    {
        opponentScore++;
        UpdateScore();
    }

    private void UpdateScore()
    {
        soundCheers.Play();
        soundCheers.time = 0f; 
        soundCheers.loop = false; 
        StartCoroutine(StopCheersAfterDuration(13f)); 

        scoreText.text = "Score: " + myScore.ToString() + "-" + opponentScore.ToString();
        goalTextColorAlpha = 1f;

    }

    private IEnumerator StopCheersAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        soundCheers.Stop(); 
    }

    private IEnumerator ShowInstructions()
    {
        yield return DisplayMessage("Practice your kicking before moving on in training", 3f);
        yield return DisplayMessage("Make at least 3 shots!", 3f);
    }

    private void DisplayThreeShotsMessage()
    {
        StartCoroutine(DisplayMessage("You made all three shots!", 3f));
    }

    public IEnumerator DisplayBallRespawnMessage()
    {
        while (goalTextColorAlpha > 0)
        {
            yield return null;
        }
        yield return DisplayMessage("Ball respawned, check center circle!", 3f);
    }

    private IEnumerator DisplayMessage(string message, float duration)
    {
        instructionText.text = message;
        instructionText.alpha = 1f;

        yield return new WaitForSeconds(duration);

        float fadeDuration = 1f;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            instructionText.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }

        instructionText.alpha = 0f;
    }
}
