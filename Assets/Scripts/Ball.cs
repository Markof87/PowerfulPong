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
    private int maxScore;

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

        rigidbody.AddForce(Random.Range(6, 8), Random.Range(-4, -3), 0);
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
