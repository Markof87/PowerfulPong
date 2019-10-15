using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BallV1
{
    public class Ball : MonoBehaviour
    {
        private Rigidbody rigidbody;
        private AudioSource audioSource;

        private int scoreLeft;
        private int scoreRight;

        public Text scoreTextLeft;
        public Text scoreTextRight;

        void Start()
        {
            scoreLeft = 0;
            scoreRight = 0;
            audioSource = GetComponent<AudioSource>();
            rigidbody = GetComponent<Rigidbody>();
            StartCoroutine(StartBall());
        }

        private IEnumerator StartBall()
        {
            yield return new WaitForSeconds(3f);
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
                transform.position = Vector3.zero;
                StartCoroutine(StartBall());
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            audioSource.Play();
        }
    }
}
