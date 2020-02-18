/*
 * PowerfulPong - Container (https://github.com/Markof87/PowerfulPong/blob/master/Assets/Scripts/Container.cs)
 * Copyright (c) 2020 Markof
 * Licensed under MIT (https://github.com/Markof87/PowerfulPong/blob/master/LICENSE)
 * 
 * Container class is a base class for container prefab. This object contains (generates) a random Pill (Pill.cs), based on set of pills
 * we include in prefab inspector.
 */

using UnityEngine;

public class Container : MonoBehaviour
{
    //These are all the pills we want to generate. Since they're chosen randomly, if you want to increment probability for a specific pill,
    //you have to include it many times in the set
    [SerializeField]
    private GameObject[] pills;

    [SerializeField]
    private AudioClip destroyClip;

    [SerializeField]
    private float containerVelocity;

    private Vector3 containerDirection;

    //These are similar to the generating system (GameManager.cs), these variables manage time movement before the Container is gone.
    [SerializeField]
    private float containerDestroyPeriod;

    private float containerDestroyTime;
    private float containerDestroySelectedTime;

    private void Start()
    {
        //This is a 50% probability to start movement up instead of down, or viceversa (not so good if we implement more than two players)
        if(Random.value < 0.5f)
            containerDirection = Vector3.up;
        else
            containerDirection = Vector3.down;
    }

    private void Update()
    {
        ContainerMovement();

        //If Container is instantiated, it must be always managed by this method
        ManageDestroyContainer();
    }

    private void OnTriggerEnter(Collider other)
    {
        //If the Ball hurts container, it'll be gone and generates a random Pill that goes to the Paddle
        if (other.gameObject.tag == "Ball")
        {
            AudioSource.PlayClipAtPoint(destroyClip, Camera.main.transform.position);
            Destroy(gameObject);
            GeneratePill(other.gameObject.GetComponent<Ball>().GetLastHit());
        }
    }

    private void ManageDestroyContainer()
    {
        //See GameManager.cs (containerSpawnerPeriod) to get the same functionality
        if (containerDestroySelectedTime == 0)
            containerDestroySelectedTime = Random.Range(5.0f, containerDestroyPeriod);

        containerDestroyTime += Time.deltaTime;
        if (containerDestroyTime >= containerDestroySelectedTime)
            Destroy(gameObject);
    }

    //Function that manages periodic movement from up to down and viceversa
    private void ContainerMovement()
    {
        if (transform.position.y >= 12f)
            containerDirection = Vector3.down;
        if(transform.position.y <= -12f)
            containerDirection = Vector3.up;

        transform.Translate(containerDirection * containerVelocity * Time.deltaTime);
    }

    //Function generated Pill randomly from his set.
    //You can potentially created any prefab variants with different sets of pills (and meshes, so will be visually different), maybe
    //some of them, more rare, with more powerful pills, or any other with trap pills. Each type of Container can have different sets
    private void GeneratePill(Paddle lastHit)
    {
        Vector3 pillPosition, pillForce;
        switch (lastHit.type)
        {
            //This is already prepared for 4 players, but it still works with 2 right now.
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

        //Choose Pill randomly from set and instantiate it
        int pillSeed = Random.Range(0, pills.Length);
        GameObject instantiatedPill = Instantiate(pills[pillSeed], pillPosition, Quaternion.identity);

        //Because the object has a Rigidbody component, this time we don't use vector translate, but that physic component.
        //It's a choice, you can uniform all of the prefabs and remove rigidbody from here, but you have to manage movement using vectors
        //(in the Pill class, on Update method)
        instantiatedPill.GetComponent<Rigidbody>().AddForce(pillForce * 500f);
    }
}
