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
            Destroy(gameObject);
            GeneratePill(other.gameObject.GetComponent<Ball>().GetLastHit());
        }
    }

    private void GeneratePill(Paddle lastHit)
    {
        Vector3 pillPosition, pillForce, pillAngularForce;
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

        GameObject instantiatedPill = Instantiate(pill, pillPosition, Quaternion.identity);
        instantiatedPill.GetComponent<Rigidbody>().AddForce(pillForce * 500f);
    }
}
