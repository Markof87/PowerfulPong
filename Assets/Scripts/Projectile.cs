/*
 * PowerfulPong - Projectile (https://github.com/Markof87/PowerfulPong/blob/master/Assets/Scripts/Projectile.cs)
 * Copyright (c) 2020 Markof
 * Licensed under MIT (https://github.com/Markof87/PowerfulPong/blob/master/LICENSE)
 * 
 * Projectile class used to manage the Ice Projectile behaviour. You can use this class like an abstract class, and extend with 
 * various type of projectiles, if you want to create more types of weapons in this game.
 * You can do with a "slower" projectile, a fire projectile (so you can implement a damage system for the paddles), 
 * an accelerator projectile for the ball (maybe it's not a great game design choice??) and anything else.
 * For now, in this first version, I use only Ice Projectile (view Readme)
 */

using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speedProjectile;

    [SerializeField]
    private AudioClip impactIceClip;

    //When I spawn the projectile, it moves to the opposite paddle direction
    private void Update()
    {
        transform.Translate(transform.right * speedProjectile * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        //When I hurt opponent paddle with projectile, the freeze routine starts (reproducing the impact sound)
        if(other.gameObject.tag == "Paddle")
        {
            AudioSource.PlayClipAtPoint(impactIceClip, Camera.main.transform.position);
            other.gameObject.GetComponent<Paddle>().HurtByIceProjectile();
        }

        if (other.gameObject.tag != "Container" && other.gameObject.tag != "Ball")
            Destroy(gameObject);
    }
}
