using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speedProjectile;

    [SerializeField]
    private AudioClip impactIceClip;

    private void Update()
    {
        transform.Translate(transform.right * speedProjectile * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Paddle")
        {
            AudioSource.PlayClipAtPoint(impactIceClip, Camera.main.transform.position);
            other.gameObject.GetComponent<Paddle>().HurtByIceProjectile();
        }

        if (other.gameObject.tag != "Container" && other.gameObject.tag != "Ball")
            Destroy(gameObject);
    }
}
