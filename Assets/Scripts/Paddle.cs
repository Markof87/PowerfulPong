/*
 * PowerfulPong - Paddle (https://github.com/Markof87/PowerfulPong/blob/master/Assets/Scripts/Paddle.cs)
 * Copyright (c) 2020 Markof
 * Licensed under MIT (https://github.com/Markof87/PowerfulPong/blob/master/LICENSE)
 * 
 * Paddle class manage paddle movements from player input
 * 
 */using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Paddle : MonoBehaviour
{
    public EventAction OnActionBehaviour;

    //Types of players expected in the game. We use only two players, but you can extend it with other 1 or 2, and maybe an AI player
    public enum PlayerType
    {
        Computer, Player1, Player2, Player3, Player4
    }

    public PlayerType type;

    //Speed of the Paddle movement and speed of the projectile you can shoot
    public int speed = 20;
    public float speedProjectile = 50;

    public bool canUseAction = true;

    //I need a ball reference so I can see when it's moving
    private Ball ball;

    [SerializeField]
    private GameObject actionIcon;

    [SerializeField]
    private GameObject iceProjectile;

    [SerializeField]
    private AudioClip shotProjectile;

    [SerializeField]
    private AudioClip useSuperSpeed;

    private void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>();
    }

    void Update()
    {
        ControllerManager();
    }

    //Manage control for all the players available (including AI, eventually)
    private void ControllerManager()
    {
        switch (type)
        {
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

    /**********************
     * MOVEMENT FUNCTIONS *
     **********************/

    private void FirstPlayer(){
        if (Input.GetKey("w") && transform.position.y < 12f)
            transform.Translate(0, speed * Time.deltaTime, 0);

        if (Input.GetKey("s") && transform.position.y > -12f)
            transform.Translate(0, -speed * Time.deltaTime, 0);

        if (Input.GetKey("e") && canUseAction)
            ActionBehaviour(); 

    }
    private void SecondPlayer(){
        if (Input.GetKey("up") && transform.position.y < 12f)
            transform.Translate(0, speed * Time.deltaTime, 0);

        if (Input.GetKey("down") && transform.position.y > -12f)
            transform.Translate(0, -speed * Time.deltaTime, 0);
    
        if (Input.GetKey(KeyCode.Return) && canUseAction)
            ActionBehaviour(); 
    }

    //TODO: these ones must be implemented, if you want to use them
    private void ThirdPlayer(){}
    private void FourthPlayer(){}
    private void AIPlayer(){}

    public GameObject GetActionIcon()
    {
        return actionIcon;
    }

    private void ActionBehaviour()
    {
        //If I have an icon image, it means I already get a Pill, so I can use correspondent action
        if(actionIcon.GetComponent<Image>().sprite != null && ball.IsMoving)
        {
            string iconName = actionIcon.GetComponent<Image>().sprite.name;
            actionIcon.GetComponent<Image>().sprite = null;

            //Delegate in the inspector (if you want to use an abstract class and call the extended function for every action possible
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

    //Start coroutine when the paddle hurted by ice projectile
    public void HurtByIceProjectile()
    {
        StartCoroutine(FreezePaddle());
    }

    //Start coroutine when the speed increase action is activated
    private void SpeedPillAction()
    {
        AudioSource.PlayClipAtPoint(useSuperSpeed, Camera.main.transform.position);
        StartCoroutine(IncreaseSpeed());
    }

    private void IcePillAction()
    {
        AudioSource.PlayClipAtPoint(shotProjectile, Camera.main.transform.position);
        ShotIceProjectile();
    }

    private IEnumerator IncreaseSpeed()
    {
        //When I activate this action, mesh color becomes kind of green and speed doubles up
        GetComponent<Renderer>().material.SetColor("_Color", new Color(0.667f, 1, 0.278f));
        speed *= 2;

        //After 10 seconds, the Paddle restored its normal movement
        yield return new WaitForSeconds(10f);
        GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 1, 1));
        speed /= 2;
    }

    private IEnumerator FreezePaddle()
    {
        //When I activate this action, mesh color becomes kind of blue ice and speed goes to zero
        GetComponent<Renderer>().material.SetColor("_Color", new Color(0, 0.639f, 1));
        speed = 0;

        //Paddle can't even use actions, if it has one
        canUseAction = false;

        //After 10 seconds, the Paddle restored its normal movement
        yield return new WaitForSeconds(5f);
        GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 1, 1));
        speed = 20;

        //And can use action again
        canUseAction = true;
    }

    private void ShotIceProjectile()
    {
        //Instantiate projectile object just a little further on, so I can avoid any ugly collision with paddle itself
        Vector3 projectileSpawn = transform.position + (1.5f * transform.right);
        GameObject projectile = Instantiate(iceProjectile, projectileSpawn, Quaternion.identity);

        //Just rotate the particle effect on the same direction
        projectile.transform.GetChild(1).gameObject.transform.rotation = transform.localRotation;

        //Set the speed with speed projectile above
        projectile.GetComponent<Projectile>().speedProjectile = speedProjectile * transform.right.x;
    }

    [System.Serializable]
    public class EventAction : UnityEvent<string> { }
}
