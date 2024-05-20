using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [Header("Door Attributes")]
   [SerializeField] Sprite openingDoorSprite;
    SpriteRenderer doorSpriteRenderer;
    BoxCollider2D doorCollider;

    [Header("Show Path")]
    [SerializeField] GameObject gameObjectToHide;
    
    void Awake()
    {
        doorSpriteRenderer = GetComponent<SpriteRenderer>();
        doorCollider = GetComponent<BoxCollider2D>();
    }

    public void OpenDoor()
    {
        // Show the Door Opening Sprite
        doorSpriteRenderer.sprite = openingDoorSprite;

        // Turn off the door Collider
        doorCollider.isTrigger = true;
    }

    // When the Player pass through the door
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameObjectToHide.SetActive(false);
        }
    }
}
