using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public enum PlayerType
    {
        Computer, Player1, Player2, Player3, Player4
    }

    public PlayerType type;

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
    }
    private void SecondPlayer(){
        if (Input.GetKey("up") && transform.position.y < 12f)
            transform.Translate(0, 20 * Time.deltaTime, 0);

        if (Input.GetKey("down") && transform.position.y > -12f)
            transform.Translate(0, -20 * Time.deltaTime, 0);
    }
    private void ThirdPlayer(){
        
    }
    private void FourthPlayer(){
        
    }
    private void AIPlayer(){
        
    }
}
