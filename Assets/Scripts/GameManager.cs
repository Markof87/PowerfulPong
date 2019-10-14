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

    public static GameManager instance = null;

    public int scoreLeft;
    public int scoreRight;
    public int scoreUp;
    public int scoreDown;
    public int maxScore = 11;

    public Text scoreTextLeft;
    public Text scoreTextRight;
    public Text winText;

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
                //scoreTextUp.text = scoreUp.ToString();
                break;

            case ScoreType.Down:
                scoreDown++;
                //scoreTextDown.text = scoreDown.ToString();
                break;
        }
    }
}
