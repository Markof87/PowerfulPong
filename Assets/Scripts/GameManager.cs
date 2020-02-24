/*
 * PowerfulPong - GameManager (https://github.com/Markof87/PowerfulPong/blob/master/Assets/Scripts/GameManager.cs)
 * Copyright (c) 2020 Markof
 * Licensed under MIT (https://github.com/Markof87/PowerfulPong/blob/master/LICENSE)
 * 
 * Main class manager of this little game. This is where I manage scoring, victory conditions 
 * and spawning pill containers when they're available.
 * This is prepared for managing Game States and more than two players too.
 */

using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //This is a functional boolean parameter, this unlock all the spawning containers manager.
    //When it's true, I can spawn containers during the game, else will behave like a normal pong (with pretty graphic, right? :D )
    public bool isImproved = true;

    public enum ScoreType
    {
        Left, Right, Up, Down
    }

    //Now this is an unused variable representing Game States. If you want to extend my game with a State Machine with a menu, pause
    //game loop and other functionalities, you can start from here!
    public enum GameState
    {
        FETCH, RUNNING, PAUSED, FINISHED
    }

    //This is for Singleton Pattern (you'll see below)
    public static GameManager instance = null;

    private int scoreLeft, scoreRight, scoreUp, scoreDown;

    //This variable is serialized so I can see (and change) it on item inspector in Unity. By default it values 11, like the original Pong
    [SerializeField]
    private int maxScore = 11;

    //All Text objects I can assigne in the inspector.
    [SerializeField]
    private Text scoreTextLeft;
    [SerializeField]
    private Text scoreTextRight;
    [SerializeField]
    private Text scoreTextUp;
    [SerializeField]
    private Text scoreTextDown;

    //Same for winning text
    [SerializeField]
    private Text winText;

    //Include in the inspector the Container object to spawn, when it's enabled
    [SerializeField]
    private Container containerToSpawn;

    //These following three variables permit to manage spawning container period. I can put a value in the inspector, and manage 
    //that by other two supporting variables
    [SerializeField]
    private float containerSpawnerPeriod;

    private float containerSpawnerTime;
    private float containerSpawnerSelectedTime;

    void Awake()
    {
        //SINGLETON PATTERN: I guarantee one and only one GameManager instance during all the game loop

        //Check if instance already exists
        if(instance == null)
            instance = this;
            
        //If instance already exists and it's not this one, then destroy it.
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        //I start the game with score and spawning time reset (value equals to 0)
        ResetGame();
    }

    void Update()
    {
        //When it's true, the spawning container system is unlocked
        if (isImproved)
            ManageSpawnContainer();
    }

    //This function activates win text (with correct content) when the game is finished
    public void ToggleWinText(bool isActive)
    {
        if (winText.IsActive() != isActive)
        {
            if (scoreLeft == maxScore)
                winText.text = "Player 1 Wins";
            else if (scoreRight == maxScore)
                winText.text = "Player 2 Wins";

            winText.gameObject.SetActive(isActive);
        }
    }

    //Reset all the score values
    public void ResetGame()
    {
        ResetSpawnTime();

        scoreLeft = 0;
        scoreRight = 0;
        scoreUp = 0;
        scoreDown = 0;

        scoreTextLeft.text = scoreLeft.ToString();
        scoreTextRight.text = scoreRight.ToString();
        scoreTextUp.text = scoreUp.ToString();
        scoreTextDown.text = scoreDown.ToString();
    }

    private void ManageSpawnContainer()
    {
        //GameManager choose a value from 5 to containerSpawnerPeriod (max value, I can change it from inspector)
        if (containerSpawnerSelectedTime == 0)
            containerSpawnerSelectedTime = Random.Range(5.0f, containerSpawnerPeriod);

        containerSpawnerTime += Time.deltaTime;

        //When time reachs the random value chosen above, GameManager spawns a new container and reset spawner time value
        if (containerSpawnerTime >= containerSpawnerSelectedTime)
        {
            ResetSpawnTime();
            SpawnContainer();
        }
    }

    //Reset spawn time and spawn time selected randomly
    private void ResetSpawnTime()
    {
        containerSpawnerTime = 0;
        containerSpawnerSelectedTime = 0;
    }

    //Function evaluates ScoreType when someone scores a point, and update total score and UI text on screen
    public void IncrementScore(ScoreType score)
    {
        DeleteAll();
        switch(score)
        {
            case ScoreType.Left:
                scoreLeft++; 
                scoreTextLeft.text = scoreLeft.ToString();
                break;

            case ScoreType.Right:
                scoreRight++;
                scoreTextRight.text = scoreRight.ToString();
                break;
                
            case ScoreType.Up:
                scoreUp++;
                scoreTextUp.text = scoreUp.ToString();
                break;

            case ScoreType.Down:
                scoreDown++;
                scoreTextDown.text = scoreDown.ToString();
                break;
        }
    }

    //Checking the win condition every time someone scores
    public bool WinCondition()
    {
        bool hasWin = (scoreLeft == maxScore || scoreRight == maxScore || scoreUp == maxScore || scoreDown == maxScore);
            
        if(hasWin)
            ToggleWinText(true);

        return hasWin;
    }

    //Instantiate a Container in the center of the screen
    private void SpawnContainer()
    {
        //TODO: maybe, instead of Vector3.zero, I can elaborate the vector y axis randomly??
        Instantiate(containerToSpawn, Vector3.zero, Quaternion.identity);
    }

    //Delete all the elements on screen (Containers and Pills)
    private void DeleteAll()
    {
        ResetSpawnTime();

        //Delete all Containers on screen
        foreach (Container container in FindObjectsOfType<Container>())
            Destroy(container.gameObject);

        //Delete all Pills on screen
        foreach (Pill pill in FindObjectsOfType<Pill>())
            Destroy(pill.gameObject);
    }
}
