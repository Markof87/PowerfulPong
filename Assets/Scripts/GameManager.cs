using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum ScoreType
    {
        Left, Right, Up, Down
    }

    public enum GameState
    {
        FETCH, RUNNING, PAUSED, FINISHED
    }

    public static GameManager instance = null;

    private int scoreLeft;
    private int scoreRight;
    private int scoreUp;
    private int scoreDown;
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

    public void ToggleWinText(bool isActive)
    {
        if (winText.IsActive() != isActive)
            winText.gameObject.SetActive(isActive);
    }

    public void ResetGame()
    {
        scoreLeft = 0;
        scoreRight = 0;
        scoreUp = 0;
        scoreDown = 0;

        scoreTextLeft.text = scoreLeft.ToString();
        scoreTextRight.text = scoreRight.ToString();
        scoreTextUp.text = scoreUp.ToString();
        scoreTextDown.text = scoreDown.ToString();
    }

    public void IncrementScore(ScoreType score)
    {
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
}
