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
    public float speedProjectile = 50;

    public bool canUseAction = true;

    [SerializeField]
    private Ball ball;

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

    public void HurtByIceProjectile()
    {
        StartCoroutine(FreezePaddle());
    }

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
        GetComponent<Renderer>().material.SetColor("_Color", new Color(0.667f, 1, 0.278f));
        speed *= 2;
        yield return new WaitForSeconds(10f);
        GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 1, 1));
        speed /= 2;
    }

    private IEnumerator FreezePaddle()
    {
        //Modify color material
        GetComponent<Renderer>().material.SetColor("_Color", new Color(0, 0.639f, 1));
        speed = 0;
        canUseAction = false;
        yield return new WaitForSeconds(5f);
        GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 1, 1));
        speed = 20;
        canUseAction = true;
    }

    private void ShotIceProjectile()
    {
        Vector3 projectileSpawn = transform.position + (1.5f * transform.right);
        GameObject projectile = Instantiate(iceProjectile, projectileSpawn, Quaternion.identity);
        projectile.transform.GetChild(1).gameObject.transform.rotation = transform.localRotation;
        projectile.GetComponent<Projectile>().speedProjectile = speedProjectile * transform.right.x;
    }

    [System.Serializable]
    public class EventAction : UnityEvent<string> { }
}
