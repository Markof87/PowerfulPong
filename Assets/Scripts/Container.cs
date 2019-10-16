using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    [SerializeField]
    private GameObject pill;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ball")
        {
            Debug.Log(other.gameObject.transform.position.x + " " + gameObject.transform.position.x);
            Destroy(gameObject);
            GeneratePill(other.gameObject.GetComponent<Ball>().GetLastHit());
        }
    }

    private void GeneratePill(Paddle lastHit)
    {
        Vector3 pillPosition;
        switch(lastHit.type)
        {
            case Paddle.PlayerType.Player1:
                pillPosition = new Vector3(transform.position.x - 2.0f, transform.position.y, 0);
                break;
            case Paddle.PlayerType.Player2:
                pillPosition = new Vector3(transform.position.x + 2.0f, transform.position.y, 0);
                break;
            case Paddle.PlayerType.Player3:
                pillPosition = new Vector3(transform.position.x, transform.position.y - 2.0f, 0);
                break;
            case Paddle.PlayerType.Player4:
                pillPosition = new Vector3(transform.position.x, transform.position.y + 2.0f, 0);
                break;
            default:
                pillPosition = new Vector3(transform.position.x, transform.position.y, 0);
                break;
        }

        Instantiate(pill, pillPosition, Quaternion.identity);
    }
}
