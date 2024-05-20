using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    MusicClient musicClient;

    [Header("File to track all music")]
    [SerializeField] string jsonFileName;
    [SerializeField] string musicFileName;
    AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();
        musicClient = GetComponent<MusicClient>();
        audioSource = GetComponent<AudioSource>();

        // Assign the right Path to store the file
        jsonFileName = $"{Application.persistentDataPath}/{jsonFileName}";
        musicFileName = $"{Application.persistentDataPath}/{musicFileName}";
    }

    public IEnumerator PlayMusicFromBinaryFile(int songID)
    {
        // Request For Song From Server
        musicClient.SendMessageToServer(songID.ToString());

        // Get the File and Save it
        yield return StartCoroutine(musicClient.ReceiveFile(musicFileName));

        // Debug.Log("Retrieve Finished!");

        // Read the binary file into a byte array
        byte[] audioBytes = File.ReadAllBytes(musicFileName);

        yield return null;

        // Convert the byte array into an audio clip
        AudioClip audioClip = MusicHelper.BytesToAudioClip(audioBytes, 44100, 2);

        yield return null;

        // Play the audio clip
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    /// <summary>
    ///     For Getting All the Song list From the Server
    /// </summary>
    public void GetListOfMusicFromServer()
    {
        // Check If Connected
        if (!musicClient.IsConnected()) return;

        // Send Request all to the Server
        musicClient.SendMessageToServer("all");

        // Get the File and Save it
        StartCoroutine(musicClient.ReceiveFile(jsonFileName));
    }

    /// <summary>
    ///     Read All the Music From the JSON File
    /// </summary>
    /// <returns></returns>
    public MusicData[] GetListOfMusicFromFile()
    {
        // Try to Read that File
        return JsonHelper.GetJsonArrayFromJsonFile<MusicData>(jsonFileName);
    }
    public void StopMusic() => audioSource.Stop();
    public void SetVolume(float volume) => audioSource.volume = volume;
    public bool Mute
    {
        get => audioSource.mute;
        set => audioSource.mute = value;
    }

}
