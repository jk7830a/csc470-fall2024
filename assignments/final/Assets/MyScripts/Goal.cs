using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
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

    [SerializeField] private Player scriptPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals("Ball"))
        {
             if (other.gameObject.GetComponent<Ball>().HasBeenBlocked)
            {
                Debug.Log("Ball blocked by goalie! No goal.");
                return;
            }

            if(name.Equals("OpponentGoalDetector"))
            {
                scriptPlayer.IncreaseMyScore();
                Debug.Log("Goal for player!");
            }
            else{
                scriptPlayer.IncreaseOpponentScore();
                Debug.Log("Goal for opponent!");
            }
        }
    }
}
