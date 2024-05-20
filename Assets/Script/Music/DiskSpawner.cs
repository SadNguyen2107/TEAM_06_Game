using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskSpawner : MonoBehaviour
{
    [Header("Raw Animation")]
    [SerializeField] Sprite stepOffSprite;
    [SerializeField] Sprite stepOnSprite;
    SpriteRenderer spriteRenderer;

    [Header("Disk to Spawn")]
    [SerializeField] GameObject diskPrefab;

    int currentId = -1;
    MusicData[] listOfMusic;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {   
        // Init Get the list of Music From the Server
        AudioManager.Instance.GetListOfMusicFromServer();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Serialize the Data From that JSON
        listOfMusic ??= AudioManager.Instance.GetListOfMusicFromFile();

        // Change the Sprite 
        spriteRenderer.sprite = stepOnSprite;

        // If Player come
        if (other.CompareTag("Player"))
        {
            // Try to Get The Appropriate ID
            if (currentId + 1 >= listOfMusic.Length) return;

            currentId++;

            // Spawner a Disk at that position
            GameObject diskGameObject = Instantiate(diskPrefab!, transform.position, Quaternion.identity);

            if (diskGameObject.TryGetComponent<MusicDisk>(out MusicDisk musicDisk))
            {
                // Set the ID, Name from the file
                musicDisk.ID = listOfMusic[currentId].ID;
                musicDisk.Name = listOfMusic[currentId].Name;

                // Set For the Text
                musicDisk.SetNameForDisk(listOfMusic[currentId].Name);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        spriteRenderer.sprite = stepOffSprite;
    }
}
