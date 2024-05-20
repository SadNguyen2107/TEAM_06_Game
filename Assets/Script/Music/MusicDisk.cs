using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MusicDisk : MonoBehaviour
{
    [SerializeField] Animator diskAnimator;
    [SerializeField] TextMeshProUGUI _diskSongText;

    public int ID { get; set; }
    public string Name { get; set; }

    public void SetNameForDisk(string name) => _diskSongText.text = name;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Altar"))
        {
            // Play the Music
            StartCoroutine(AudioManager.Instance.PlayMusicFromBinaryFile(ID));           // Main
            
            // Play the Animation
            // Debug.Log("Is Playing!");
            diskAnimator.SetTrigger("Toggle");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Altar"))
        {
            // Debug.Log("Turn Off!");
            diskAnimator.SetTrigger("Toggle");

            // Stop the Music
            AudioManager.Instance.StopMusic();
        }
    }
}
