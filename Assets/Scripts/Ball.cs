/*
 * PowerfulPong - Ball (https://github.com/Markof87/PowerfulPong/blob/master/Assets/Scripts/Ball.cs)
 * Copyright (c) 2020 Markof
 * Licensed under MIT (https://github.com/Markof87/PowerfulPong/blob/master/LICENSE)
 * 
 * Ball class is used to move the ball with physics component (we use Rigidbody again), with some of FX and audio effects
 */

using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;
    private AudioSource audioSource;

    //I keep track of the last Paddle hurt the Ball 
    private Paddle lastHit = null;

    //Keep track of the number of hits with paddles, so you can use it to accelerate velocity after N hits
    private int totalHit = 0;
    private bool isMoving = false;

    [SerializeField]
    private float initialVelocity = 5.0f;

    //Sound set of many hurts SFX, so I can choose one of them randomly before I reproduce it
    [SerializeField]
    private AudioClip[] hurtClips;

    [SerializeField]
    private AudioClip loseBallClip;

    [SerializeField]
    private GameObject explosion;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

        StartCoroutine(StartBall());
    }

    public bool IsMoving
    {
        get
        {
            return isMoving;
        }
    }

    //At start routine, this function applies a force to the Ball with a random direction
    public void AddForceBall()
    {
        float randomX = 0;
        float randomY = 0;

        //Choose if the Ball goes left or right
        if (Random.value < 0.5f)
            randomX = Random.Range(-4, -3);
        else
            randomX = Random.Range(3, 4);

        //Preserve the angle of the direction using vector operations
        if (randomX <= -3)
            randomY = -Mathf.Sqrt(Mathf.Pow(initialVelocity, 2) - Mathf.Pow(randomX, 2));
        else
            randomY = Mathf.Sqrt(Mathf.Pow(initialVelocity, 2) - Mathf.Pow(randomX, 2));

        //Add force vector to the Ball
        Vector3 randomStart = new Vector3(randomX, randomY, 0);
        rb.AddForce(randomStart);
    }

    public Paddle GetLastHit()
    {
        return lastHit;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Boundary")
        {
            //Increment score if the Ball goes out of bounds.
            //This isn't good if you have more of 2 players. If you want to create other 1-2 players, you have to increment score
            //to the last Paddle touch the ball.
            //
            //Or maybe you can do all the contrary, set all the points to 11 and decrement score every time player lose the ball.
            //When a player reach zero points, it'll be eliminated, until only one player remains in game.
            if (other.gameObject.name == "BoundaryLeft")
                GameManager.instance.IncrementScore(GameManager.ScoreType.Right);

            if (other.gameObject.name == "BoundaryRight")
                GameManager.instance.IncrementScore(GameManager.ScoreType.Left);

            if (other.gameObject.name == "BoundaryUp")
                GameManager.instance.IncrementScore(GameManager.ScoreType.Up);

            if (other.gameObject.name == "BoundaryDown")
                GameManager.instance.IncrementScore(GameManager.ScoreType.Down);

            //If the ball goes out of bounds, it has to be exploded and reset the position on the centre of the screen
            ReproduceLoseSound();
            ExplodeBall(other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position));

            //Reset forces
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            //Check if one of the players wins
            if(!GameManager.instance.WinCondition())
                StartCoroutine(StartBall());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ReproduceHurtSound();

        //On every collision I have to count the total amount. If i reach 4, 12 and 20 hits, my ball is more quick by a bit
        if(collision.gameObject.tag == "Paddle")
        {
            lastHit = collision.gameObject.GetComponent<Paddle>();
            ManageDeflection(collision);

            totalHit++;

            //When Ball reaches 4, 12 or 20 rebounds with Paddle, it'll accelerate through a specific direction.
            if(totalHit == 4)
            {
                if(rb.velocity.x <= 0)
                    rb.velocity = new Vector3(rb.velocity.x - 5.0f, rb.velocity.y, 0);
                else
                    rb.velocity = new Vector3(rb.velocity.x + 5.0f, rb.velocity.y, 0);
            }

            if(totalHit == 12)
            {
                if(rb.velocity.x <= 0)
                    rb.velocity = new Vector3(rb.velocity.x - 10.0f, rb.velocity.y, 0);
                else
                    rb.velocity = new Vector3(rb.velocity.x + 10.0f, rb.velocity.y, 0);
            }

            if(totalHit == 20)
            {
                if(rb.velocity.x <= 0)
                    rb.velocity = new Vector3(rb.velocity.x - 20.0f, rb.velocity.y, 0);
                else
                    rb.velocity = new Vector3(rb.velocity.x + 20.0f, rb.velocity.y, 0);    
            }
        }
    }

    //Start routine
    private IEnumerator StartBall()
    {
        //Reset moving and total hits
        isMoving = false;
        transform.position = Vector3.zero;
        totalHit = 0;
        GameManager.instance.ToggleWinText(false);

        yield return new WaitForSeconds(3f);

        //After three seconds (maybe I should serialize this value??), start moving the ball
        AddForceBall();
        isMoving = true;
    }

    //Elaborate the right angle every time the Ball hits a Paddle
    private void ManageDeflection(Collision collision)
    {
        float differencePosition = collision.transform.position.y - transform.position.y;

        Vector3 dir = Quaternion.AngleAxis(60 * differencePosition, Vector3.forward) * Vector3.right;
        rb.AddForce(dir * 1.0f);
    }

    /*************************
     * VFX AND SFX FUNCTIONS *
     *************************/

    private void ExplodeBall(Vector3 explosionPosition)
    {
        GameObject explosionObject = Instantiate(explosion, explosionPosition, Quaternion.identity);
        Destroy(explosionObject, 1.5f);
    }

    private void ReproduceHurtSound()
    {
        audioSource.PlayOneShot(hurtClips[Random.Range(0, hurtClips.Length)]);
        audioSource.Play();
    }

    private void ReproduceLoseSound()
    {
        audioSource.PlayOneShot(loseBallClip);
        audioSource.Play();
    }
}
