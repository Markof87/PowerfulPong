using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pill : MonoBehaviour
{
    [SerializeField]
    private Sprite actionIconSprite;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Paddle")
        {
            GameObject actionIcon = other.gameObject.GetComponent<Paddle>().GetActionIcon();
            actionIcon.GetComponent<Image>().sprite = actionIconSprite;
        }

        if(other.gameObject.tag != "Container" && other.gameObject.tag != "Ball")
            Destroy(gameObject);
    }


    //N.B: I can do this pattern really better than that. But I think is enough for this little project
    public void ExecuteAction(string pillName, Paddle executorPaddle)
    {
        switch (pillName)
        {
            case "speed":
                SpeedPillAction(executorPaddle);
                break;
            case "ice":
                IcePillAction(executorPaddle);
                break;
        }
    }

    private void SpeedPillAction(Paddle executorPaddle)
    {
        Debug.Log("I'm executing speed action");
    }

    private void IcePillAction(Paddle executorPaddle)
    {
        Debug.Log("I'm executing ice action");
    }
}
