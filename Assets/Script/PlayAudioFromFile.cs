using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
public class PlayAudioFromFile : MonoBehaviour
{
    private AudioSource audioSource; // reference to the audio source component
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayTrack(string audioFileNme)
    {
        if (!audioFileNme.ToLower().EndsWith(".wav"))
        {
            audioFileNme += ".wav";
        }
        string filepath = Path.Combine(Application.persistentDataPath, audioFileNme);
        Debug.Log("Play clip to " + filepath);
        // Make sure the directory exists
        Directory.CreateDirectory(Path.GetDirectoryName(filepath));
        // Load the audio clip from the file path
        AudioClip audioClip = LoadAudioClip(filepath);
        // Set the audio clip to the audio source
        audioSource.clip = audioClip;
        // Play the audio clip
        audioSource.Play();
        Debug.Log("audioSource.name " + audioSource.name);
    }
    private AudioClip LoadAudioClip(string path)
    {
        // Load the audio clip from the file path using WWW class
        Debug.Log("path " + path);
        WWW audioLoader = new WWW("file://" + path); //WW is used for unity web request 
        // Wait until the audio clip is completely loaded
        Debug.Log("path " + path);
        Debug.Log("audioSource.name " + audioLoader);

        while (!audioLoader.isDone) { }
        // Return the loaded audio clip
        return audioLoader.GetAudioClip(false, false);
    }
}