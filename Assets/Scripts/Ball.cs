using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;
    private AudioSource audioSource;
    private Paddle lastHit = null;

    private int totalHit = 0;
    private bool isMoving = false;

    [SerializeField]
    private float initialVelocity = 5.0f;

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

    public void AddForceBall()
    {
        float randomX = 0;
        float randomY = 0;

        if (Random.value < 0.5f)
            randomX = Random.Range(-4, -3);
        else
            randomX = Random.Range(3, 4);

        if (randomX <= -3)
            randomY = -Mathf.Sqrt(Mathf.Pow(initialVelocity, 2) - Mathf.Pow(randomX, 2));
        else
            randomY = Mathf.Sqrt(Mathf.Pow(initialVelocity, 2) - Mathf.Pow(randomX, 2));

        Vector3 randomStart = new Vector3(randomX, randomY, 0);
        rb.AddForce(randomStart);
    }

    private IEnumerator StartBall()
    {
        isMoving = false;
        transform.position = Vector3.zero;
        totalHit = 0;

        yield return new WaitForSeconds(3f);

        GameManager.instance.ToggleWinText(false);
        AddForceBall();
        isMoving = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Boundary")
        {

            if (other.gameObject.name == "BoundaryLeft")
                GameManager.instance.IncrementScore(GameManager.ScoreType.Right);

            if (other.gameObject.name == "BoundaryRight")
                GameManager.instance.IncrementScore(GameManager.ScoreType.Left);

            if (other.gameObject.name == "BoundaryUp")
                GameManager.instance.IncrementScore(GameManager.ScoreType.Up);

            if (other.gameObject.name == "BoundaryDown")
                GameManager.instance.IncrementScore(GameManager.ScoreType.Down);

            ReproduceLoseSound();
            ExplodeBall(other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position));

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

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

        Vector3 dir = Quaternion.AngleAxis(60 * differencePosition, Vector3.forward) * Vector3.right;
        rb.AddForce(dir * 1.0f);
    }

    public Paddle GetLastHit()
    {
        return lastHit;
    }

    private void ExplodeBall(Vector3 explosionPosition)
    {
        GameObject explosionObject = Instantiate(explosion, explosionPosition, Quaternion.identity);
        Destroy(explosionObject, 1.5f);
        //explosion.GetComponent<ParticleSystem>().Play();
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
