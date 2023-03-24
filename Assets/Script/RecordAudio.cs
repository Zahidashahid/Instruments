using UnityEngine;
using System.Collections;
using TMPro;
// Require an AudioSource component to be attached to the same GameObject
[RequireComponent(typeof(AudioSource))]
public class RecordAudio : MonoBehaviour
{
    // Variables to keep track of recording state and recording time
    private bool isRecording = false;
    private bool isPaused = false;
    // Audio-related variables
    private AudioSource audioSource;
    private AudioClip recordedClip;
    string trackName;
    public TMP_Text recordingTimeText;  // Reference to the UI Text object to display recording time
    public float recordingTime = 0f;  // current recording time
    void Start()
    {
        // Get the attached AudioSource component
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if(isRecording && !isPaused)
        {
            // Update the recording time while recording is in progress
            recordingTime += Time.deltaTime;  
            recordingTimeText.text = "Recording : " + recordingTime.ToString("F2") + " seconds";  // update UI Text
        }
        else // if(!isRecording && !isPaused)
        {
            recordingTime = 0f; // Reset the recording time when recording is not in progress
        }
    }
    public void RecordMusic(string _trackName)
    {
        if (!isRecording)
        {
            // Start recording audio using the device's microphone
            Debug.Log("Start recording...");
            trackName = _trackName;
            recordedClip = Microphone.Start(null, false, 10, 44100);
            isRecording = true;
            isPaused = false; // make sure recording is not paused
        }
    }
    public void StopRecording()
    {
        // Stop recording and save the recorded audio clip as a WAV file
        Debug.Log("Stop recording...");
        Microphone.End(null);
        isRecording = false;
        isPaused = false; // make sure recording is not paused

        // Save the recorded clip
        SavWav.Save(trackName, recordedClip);
        Debug.Log("Clip saved.");
    }
    public void PauseRecording()
    {
        if (isRecording && !isPaused)
        {
            // Pause the recording if it is currently in progress
            Debug.Log("Pause recording...");
            isPaused = true;
        }
    }
    public void ResumeRecording()
    {
        if (isRecording && isPaused)
        {
            // Resume the recording if it was previously paused
            Debug.Log("Resume recording...");
            isPaused = false;
        }
    }
}