using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool isImproved = true;
    public enum ScoreType
    {
        Left, Right, Up, Down
    }

    public enum GameState
    {
        FETCH, RUNNING, PAUSED, FINISHED
    }

    public static GameManager instance = null;

    private int scoreLeft, scoreRight, scoreUp, scoreDown;
    private int maxScore = 11;

    [SerializeField]
    private Text scoreTextLeft;
    [SerializeField]
    private Text scoreTextRight;
    [SerializeField]
    private Text scoreTextUp;
    [SerializeField]
    private Text scoreTextDown;
    [SerializeField]
    private Text winText;

    [SerializeField]
    private Container containerToSpawn;
    [SerializeField]
    private float containerSpawnerPeriod;
    private float containerSpawnerTime;
    private float containerSpawnerSelectedTime;

    void Awake()
    {
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
        ResetGame();
    }

    void Update()
    {
        if (isImproved)
        {
            if (containerSpawnerSelectedTime == 0)
                containerSpawnerSelectedTime = Random.Range(5.0f, containerSpawnerPeriod);

            containerSpawnerTime += Time.deltaTime;
            if (containerSpawnerTime >= containerSpawnerSelectedTime)
            {
                ResetSpawnTime();
                SpawnContainer();
            }
        }
    }

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

    private void ResetSpawnTime()
    {
        containerSpawnerTime = 0;
        containerSpawnerSelectedTime = 0;
    }

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

    public bool WinCondition()
    {
        bool hasWin = (scoreLeft == maxScore || scoreRight == maxScore || scoreUp == maxScore || scoreDown == maxScore);
            
        if(hasWin)
            ToggleWinText(true);

        return hasWin;
    }

    private void SpawnContainer()
    {
        Instantiate(containerToSpawn, Vector3.zero, Quaternion.identity);
    }

    private void DeleteAll()
    {
        ResetSpawnTime();
        foreach (Container container in FindObjectsOfType<Container>())
            Destroy(container.gameObject);

        foreach (Pill pill in FindObjectsOfType<Pill>())
            Destroy(pill.gameObject);
    }
}
