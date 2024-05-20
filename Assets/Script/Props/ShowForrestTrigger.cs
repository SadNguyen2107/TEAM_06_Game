using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowForrestTrigger : MonoBehaviour
{
    [Header("Game Object to Appear Again")]
    [SerializeField] GameObject gameObjectToShow;

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameObjectToShow.SetActive(true);
        }   
    }
}
