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
    public int speed = 20;
    public int speedProjectile = 50;

    private bool projectileInstantiated = false;
    private GameObject iceProjectileSpawned = null;

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

        if (projectileInstantiated)
            MoveIceProjectile();
    }

    private void FirstPlayer(){
        if (Input.GetKey("w") && transform.position.y < 12f)
            transform.Translate(0, speed * Time.deltaTime, 0);

        if (Input.GetKey("s") && transform.position.y > -12f)
            transform.Translate(0, -speed * Time.deltaTime, 0);

        if (Input.GetKey("e"))
            ActionBehaviour(); 

    }
    private void SecondPlayer(){
        if (Input.GetKey("up") && transform.position.y < 12f)
            transform.Translate(0, speed * Time.deltaTime, 0);

        if (Input.GetKey("down") && transform.position.y > -12f)
            transform.Translate(0, -speed * Time.deltaTime, 0);
    
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
            OnActionBehaviour.Invoke(iconName);
        }
    }

    //N.B: I can do this pattern really better than that. But I think is enough for this little project
    public void ExecuteAction(string pillName)
    {
        switch (pillName)
        {
            case "speed":
                SpeedPillAction();
                break;
            case "ice":
                IcePillAction();
                break;
        }
    }

    private void SpeedPillAction()
    {
        StartCoroutine(IncreaseSpeed());
    }

    private void IcePillAction()
    {
        ShotIceProjectile();
    }

    private IEnumerator IncreaseSpeed()
    {
        speed *= 2;
        yield return new WaitForSeconds(10f);
        speed /= 2;
    }

    private void ShotIceProjectile()
    {
        iceProjectileSpawned = Instantiate(iceProjectile, transform.position, transform.localRotation);
        projectileInstantiated = true;
    }

    private void MoveIceProjectile()
    {
        iceProjectileSpawned.transform.Translate(transform.right * speedProjectile * Time.deltaTime);
    }

    [System.Serializable]
    public class EventAction : UnityEvent<string> { }
}
