using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;
    private AudioSource audioSource;

    private int totalHit = 0;

    [SerializeField]
    private float initialVelocity = 5.0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

        StartCoroutine(StartBall());
    }

    private IEnumerator StartBall()
    {
        transform.position = Vector3.zero;
        totalHit = 0;

        yield return new WaitForSeconds(3f);

        GameManager.instance.ToggleWinText(false);
        AddForceBall();
    }

    private void AddForceBall()
    {
        float randomX = 0;
        float randomY = 0;

        if (Random.value < 0.5f)
            randomX = Random.Range(-5, -3);
        else
            randomX = Random.Range(3, 5);

        if (randomX <= -3)
            randomY = -Mathf.Sqrt(Mathf.Pow(initialVelocity, 2) - Mathf.Pow(randomX, 2));
        else
            randomY = Mathf.Sqrt(Mathf.Pow(initialVelocity, 2) - Mathf.Pow(randomX, 2));

        Vector3 randomStart = new Vector3(randomX, randomY, 0);
        rb.AddForce(randomStart);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Boundary")
        {

            if (other.gameObject.name == "BoundaryLeft")
                GameManager.instance.IncrementScore(GameManager.ScoreType.Right);

            if (other.gameObject.name == "BoundaryRight")
                GameManager.instance.IncrementScore(GameManager.ScoreType.Left);

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            if(!GameManager.instance.WinCondition())
                StartCoroutine(StartBall());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        audioSource.Play();

        //On every collision I have to count the total amount. If i reach 4, 12 and 20 hits, my ball is more quick by a bit
        if(collision.gameObject.tag == "Paddle")
        {

            ManageDeflection(collision);

            totalHit++;
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

    private void ManageDeflection(Collision collision)
    {
        float differencePosition = collision.transform.position.y - transform.position.y;

        //If the difference between paddle position and ball position is less than zero, the ball is a bit higher
        //if (differencePosition < 0)
        //    rb.AddForce(1.0f * differencePosition);
        Debug.Log(collision.transform.position.y - transform.position.y);
    }
}
