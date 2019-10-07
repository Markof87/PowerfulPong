using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    private Rigidbody rigidbody;
    private AudioSource audioSource;

    private int scoreLeft;
    private int scoreRight;
    private int scoreUp;
    private int scoreDown;

    [SerializeField]
    private int maxScore = 11;

    [SerializeField]
    private float initialVelocity = 5.0f;

    public Text scoreTextLeft;
    public Text scoreTextRight;
    public Text winText;

    void Start()
    {
        ResetGame();

        audioSource = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();

        StartCoroutine(StartBall());
    }

    private IEnumerator StartBall()
    {
        transform.position = Vector3.zero;

        yield return new WaitForSeconds(3f);

        if (winText.IsActive())
            winText.gameObject.SetActive(false);

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
        rigidbody.AddForce(randomStart);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Boundary")
        {

            if (other.gameObject.name == "BoundaryLeft")
            {
                scoreRight++;
                scoreTextRight.text = scoreRight.ToString();
            }

            if (other.gameObject.name == "BoundaryRight")
            {
                scoreLeft++;
                scoreTextLeft.text = scoreLeft.ToString();
            }

            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;

            if (scoreLeft == maxScore || scoreRight == maxScore || scoreUp == maxScore || scoreDown == maxScore)
                winText.gameObject.SetActive(true);

            else
                StartCoroutine(StartBall());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //On every collision I have to count the total amount. If i reach 4, 12 and 20 hits, my ball is more quick by a bit
        audioSource.Play();
    }

    private void ResetGame()
    {
        scoreLeft = 0;
        scoreRight = 0;
        scoreUp = 0;
        scoreDown = 0;

        scoreTextLeft.text = scoreLeft.ToString();
        scoreTextRight.text = scoreRight.ToString();
    }
}
