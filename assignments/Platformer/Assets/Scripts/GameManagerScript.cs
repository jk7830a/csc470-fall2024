using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
    int totalCarrots = 5;
    int collectedCarrots = 0;
    public DayNightCycle dayNightCycle;

    public Canvas gameCanvas;
    public TextMeshProUGUI gameMessageText;

    private bool gameStarted = false;
    private bool gameEnded = false; 

    // Start is called before the first frame update
    void Start()
    {
        gameMessageText = GameObject.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        gameMessageText.text = "Press Play to Start";
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameStarted && Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }

        if(gameStarted && !gameEnded && dayNightCycle.IsNightTime())
        {
            CheckGameOutcome();
        }
        
    }

    void StartGame()
    {
        gameStarted = true;
        gameMessageText.text = "Collect 5 carrots before nightfall, but stay away from foxes and skunks!";
    }

    public void CollectCarrot()
    {
        collectedCarrots++;
        gameMessageText.text = "Carrot collected!";
    }

    public void CollectRadish()
    {
        collectedCarrots++;
        gameMessageText.text = "Not a carrot, but a radish!";
    }

    void CheckGameOutcome()
    {
        if(collectedCarrots < totalCarrots)
        {
            EndGame("You Lose! Not enough carrots collected!");
        }else{
            EndGame("You Win! You collected enough carrots!");
        }
    }

    void EndGame(string message)
    {
        gameMessageText.text = message;
        gameEnded = true;

        Time.timeScale = 0f;
    }

    public void GameOverByPredator()
    {
        EndGame("Game Over! You've been caught by a predator!");
    }
}
