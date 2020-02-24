/*
 * PowerfulPong - Pill (https://github.com/Markof87/PowerfulPong/blob/master/Assets/Scripts/Pill.cs)
 * Copyright (c) 2020 Markof
 * Licensed under MIT (https://github.com/Markof87/PowerfulPong/blob/master/LICENSE)
 * 
 * Pill class is a base class for various type of pills. In this first version I have implemented two of them: Ice and Speed Pill.
 * You can create many more types of Pills with many prefabs with Pill component in each of them. Or you can create Prefab Variants
 * from original Pill prefab, and modify (or even create new ones, with extended classes) them with different parameters in the inspector.
 */

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
        //If Paddle "catch" the Pill, the sound is played and get the action icon from the Paddle player.
        if(other.gameObject.tag == "Paddle")
        {
            AudioSource.PlayClipAtPoint(triggerAudioClip, Camera.main.transform.position);
            GameObject actionIcon = other.gameObject.GetComponent<Paddle>().GetActionIcon();

            //It gets the value from Pill
            actionIcon.GetComponent<Image>().sprite = actionIconSprite;
        }

        //If Pill trigger Paddle or boundary limit, it'll destroyed
        if(other.gameObject.tag != "Container" && other.gameObject.tag != "Ball")
            Destroy(gameObject);
    }
}
