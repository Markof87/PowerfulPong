using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Paddle : MonoBehaviour
{
    public EventAction OnActionBehaviour;

    public enum PlayerType
    {
        Computer, Player1, Player2, Player3, Player4
    }

    public PlayerType type;

    [SerializeField]
    private Ball ball;

    [SerializeField]
    private GameObject iceProjectile;

    private void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>();
    }

    [SerializeField]
    private GameObject actionIcon;

    void Update()
    {
        switch(type){
            case PlayerType.Player1:
                FirstPlayer();
                break;

            case PlayerType.Player2:
                SecondPlayer();
                break;

            case PlayerType.Player3:
                ThirdPlayer();
                break;

            case PlayerType.Player4:
                FourthPlayer();
                break;

            case PlayerType.Computer:
                AIPlayer();
                break;
        }
    }

    private void FirstPlayer(){
        if (Input.GetKey("w") && transform.position.y < 12f)
            transform.Translate(0, 20 * Time.deltaTime, 0);

        if (Input.GetKey("s") && transform.position.y > -12f)
            transform.Translate(0, -20 * Time.deltaTime, 0);

        if (Input.GetKey("e"))
            ActionBehaviour(); 

    }
    private void SecondPlayer(){
        if (Input.GetKey("up") && transform.position.y < 12f)
            transform.Translate(0, 20 * Time.deltaTime, 0);

        if (Input.GetKey("down") && transform.position.y > -12f)
            transform.Translate(0, -20 * Time.deltaTime, 0);
    
        if (Input.GetKey(KeyCode.Return))
            ActionBehaviour(); 
    }
    private void ThirdPlayer(){
        
    }
    private void FourthPlayer(){
        
    }
    private void AIPlayer(){
        
    }

    public GameObject GetActionIcon()
    {
        return actionIcon;
    }

    private void ActionBehaviour()
    {
        if(actionIcon.GetComponent<Image>().sprite != null && ball.IsMoving)
        {
            string iconName = actionIcon.GetComponent<Image>().sprite.name;
            actionIcon.GetComponent<Image>().sprite = null;
            OnActionBehaviour.Invoke(iconName, this);
        }
    }

    [System.Serializable]
    public class EventAction : UnityEvent<string, Paddle> { }
}
