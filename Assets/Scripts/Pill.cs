using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pill : MonoBehaviour
{
    [SerializeField]
    private Sprite actionIconSprite;

    [SerializeField]
    private AudioClip triggerAudioClip;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Paddle")
        {
            AudioSource.PlayClipAtPoint(triggerAudioClip, Camera.main.transform.position);
            GameObject actionIcon = other.gameObject.GetComponent<Paddle>().GetActionIcon();
            actionIcon.GetComponent<Image>().sprite = actionIconSprite;
        }

        if(other.gameObject.tag != "Container" && other.gameObject.tag != "Ball")
            Destroy(gameObject);
    }
}
