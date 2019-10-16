using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleRight : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey("up"))
            transform.Translate(0, 20 * Time.deltaTime, 0);

        if (Input.GetKey("down"))
            transform.Translate(0, -20 * Time.deltaTime, 0);
    }
}
