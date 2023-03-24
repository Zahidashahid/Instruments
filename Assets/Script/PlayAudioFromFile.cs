using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using TMPro;

public class PlayAudioFromFile : MonoBehaviour
{
    private AudioSource audioSource; // reference to the audio source component
    public TMP_Text timerText;  // reference to the time Text component
    private float startTime;   // the start time of the audio clip
    private bool isPlaying = false; // a flag indicating whether the audio clip is playing or not
    void Start()
    {
        // Get a reference to the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();
    }
   
    void Update()
    {
        // If the audio clip is playing, update the timer text to show the elapsed time 
        if (isPlaying)
        {
            float elapsedTime = Time.time - startTime;
            timerText.text = "Time elapsed: " + FormatTime(elapsedTime);
        }
    }
    public void StartTimer() // Start the timer
    {
        startTime = Time.time;
        isPlaying = true;
    }
    public void StopTimer() // Stop the timer
    {
        isPlaying = false;
    }
    // Format the time elapsed into a string of the format "MM:SS.mmm"
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time * 1000) % 1000);
        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }
    // Load and play the audio clip with the given filename
    public void PlayTrack(string audioFileNme)
    {
        // If the filename does not end with ".wav", add it to the filename
        if (!audioFileNme.ToLower().EndsWith(".wav"))
        {
            audioFileNme += ".wav";
        }
        // Combine the filename with the persistent data path to get the full file path
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
        StartTimer();
        Debug.Log("audioSource.name " + audioSource.name);
    }
    // Load an audio clip from the given file path
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