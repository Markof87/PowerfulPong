using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    [SerializeField]
    private GameObject[] pills;

    [SerializeField]
    private AudioClip destroyClip;

    [SerializeField]
    private float containerVelocity;

    [SerializeField]
    private float containerDestroyPeriod;

    private Vector3 containerDirection;

    private float containerDestroyTime;
    private float containerDestroySelectedTime;

    private void Start()
    {
        if(Random.value < 0.5f)
            containerDirection = Vector3.up;
        else
            containerDirection = Vector3.down;
    }

    private void Update()
    {
        ContainerMovement();

        if(containerDestroySelectedTime == 0)
            containerDestroySelectedTime = Random.Range(5.0f, containerDestroyPeriod);

        containerDestroyTime += Time.deltaTime;
        if(containerDestroyTime >= containerDestroySelectedTime)
            Destroy(gameObject);
    }

    private void ContainerMovement()
    {
        if (transform.position.y >= 12f)
            containerDirection = Vector3.down;
        if(transform.position.y <= -12f)
            containerDirection = Vector3.up;

        transform.Translate(containerDirection * containerVelocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ball")
        {
            AudioSource.PlayClipAtPoint(destroyClip, Camera.main.transform.position);
            Destroy(gameObject);
            GeneratePill(other.gameObject.GetComponent<Ball>().GetLastHit());
        }
    }

    private void ResetDestroyTime()
    {
        containerDestroyTime = 0;
        containerDestroySelectedTime = 0;
    }

    private void GeneratePill(Paddle lastHit)
    {
        Vector3 pillPosition, pillForce;
        switch (lastHit.type)
        {
            case Paddle.PlayerType.Player1:
                pillPosition = new Vector3(transform.position.x - 2.0f, transform.position.y, 0);
                pillForce = Vector3.left;
                break;
            case Paddle.PlayerType.Player2:
                pillPosition = new Vector3(transform.position.x + 2.0f, transform.position.y, 0);
                pillForce = Vector3.right;
                break;
            case Paddle.PlayerType.Player3:
                pillPosition = new Vector3(transform.position.x, transform.position.y - 2.0f, 0);
                pillForce = Vector3.up;
                break;
            case Paddle.PlayerType.Player4:
                pillPosition = new Vector3(transform.position.x, transform.position.y + 2.0f, 0);
                pillForce = Vector3.down;
                break;
            default:
                pillPosition = new Vector3(transform.position.x, transform.position.y, 0);
                pillForce = Vector3.zero;
                break;
        }

        int pillSeed = Random.Range(0, pills.Length);
        GameObject instantiatedPill = Instantiate(pills[pillSeed], pillPosition, Quaternion.identity);
        instantiatedPill.GetComponent<Rigidbody>().AddForce(pillForce * 500f);
    }
}
