using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleLeft : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w"))
            transform.Translate(0, 20 * Time.deltaTime, 0);

        if (Input.GetKey("s"))
            transform.Translate(0, -20 * Time.deltaTime, 0);
    }
}
