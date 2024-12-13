using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using StarterAssets;
using UnityEngine;

public class Goalie : MonoBehaviour
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

    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody ballRigidbody;
    [SerializeField] private Transform ballTransform; 

    [SerializeField] private float blockDistance = 5f;
    [SerializeField] private float diveDistance = 10f;
    [SerializeField] private float sidestepDuration = 1.0f;
    [SerializeField] private float sidestepRangeZ = 2f; 
    [SerializeField] private float reactDelay = 0.2f; 

    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool isReactingToBall = false;

    private const int ANIMATION_LAYER_SIDESTEP_RIGHT = 1;
    private const int ANIMATION_LAYER_SIDESTEP_LEFT = 2;
    private const int ANIMATION_LAYER_BLOCK = 3;
    private const int ANIMATION_LAYER_CATCH = 4;
    private const int ANIMATION_LAYER_DIVE = 5;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void Update()
    {
        Vector3 position = transform.position;
        position.y = startPosition.y; 
        transform.position = position;

        transform.rotation = startRotation; 

        if (ballRigidbody.velocity.magnitude < 0.1f && !isReactingToBall)
    {
        animator.SetLayerWeight(ANIMATION_LAYER_SIDESTEP_LEFT, 0);
        animator.SetLayerWeight(ANIMATION_LAYER_SIDESTEP_RIGHT, 0);
        animator.SetLayerWeight(ANIMATION_LAYER_BLOCK, 0);
        animator.SetLayerWeight(ANIMATION_LAYER_CATCH, 0);
        animator.SetLayerWeight(ANIMATION_LAYER_DIVE, 0);
        animator.ResetTrigger("SidestepLeft");
        animator.ResetTrigger("SidestepRight");
        animator.ResetTrigger("Block");
        animator.ResetTrigger("Catch");
        animator.ResetTrigger("Dive");

        if (transform.position != startPosition)
        {
            transform.position = startPosition; 
        }
    }

        if (!isReactingToBall && ballRigidbody.velocity.magnitude > 0.5f)
        {
            ReactToBall();
        }
    }

    private void ReactToBall()
    {
        isReactingToBall = true;
        StopAllCoroutines();

        float ballZ = ballTransform.position.z;
        float goalieZ = transform.position.z;

        if (Mathf.Abs(ballZ - goalieZ) > sidestepRangeZ)
        {
            StartCoroutine(SidestepAndReact(ballZ > goalieZ ? "SidestepRight" : "SidestepLeft"));
        }
        else
        {
            StartCoroutine(DefensiveMove());
        }
    }

    private IEnumerator SidestepAndReact(string sidestepTrigger)
    {
        animator.SetLayerWeight(sidestepTrigger == "SidestepRight" ? ANIMATION_LAYER_SIDESTEP_RIGHT : ANIMATION_LAYER_SIDESTEP_LEFT, 1);
        animator.SetTrigger(sidestepTrigger);

        float elapsedTime = 0f;
        Vector3 targetPosition = transform.position + new Vector3(0, 0, sidestepTrigger == "SidestepRight" ? sidestepRangeZ : -sidestepRangeZ);

        while (elapsedTime < sidestepDuration)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, elapsedTime / sidestepDuration);
            newPosition.y = startPosition.y;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        animator.SetLayerWeight(sidestepTrigger == "SidestepRight" ? ANIMATION_LAYER_SIDESTEP_RIGHT : ANIMATION_LAYER_SIDESTEP_LEFT, 0);

        yield return new WaitForSeconds(reactDelay);
        StartCoroutine(DefensiveMove());
    }

    private IEnumerator DefensiveMove()
    {
        float distanceToBall = Vector3.Distance(transform.position, ballTransform.position);

        if (distanceToBall < blockDistance)
        {
            TriggerBlock();
        }
        else if (distanceToBall < diveDistance)
        {
            TriggerDive();
        }
        else
        {
            isReactingToBall = false;
        }

        yield return null;
    }

    private void TriggerBlock()
    {
        animator.SetLayerWeight(ANIMATION_LAYER_BLOCK, 1);
        animator.Play("Block", ANIMATION_LAYER_BLOCK, 0f);
        Debug.Log("Goalie blocks!");

        ResetReaction();
    }

    private void TriggerCatch()
    {
        animator.SetLayerWeight(ANIMATION_LAYER_CATCH, 1);
        animator.Play("Catch", ANIMATION_LAYER_CATCH, 0f);
        Debug.Log("Goalie caught ball!");

        ResetReaction();
    }

    private void TriggerDive()
    {
        animator.SetLayerWeight(ANIMATION_LAYER_DIVE, 1);
        animator.Play("Dive", ANIMATION_LAYER_DIVE, 0f);
        Debug.Log("Goalie dives!");

        ResetReaction();
    }

    private void ResetReaction()
    {
        isReactingToBall = false;
        StartCoroutine(IdleRoutine());
    }

    private IEnumerator IdleRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        transform.position = startPosition; 
    }
}
