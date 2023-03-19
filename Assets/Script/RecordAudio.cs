using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class RecordAudio : MonoBehaviour
{

    private bool isRecording = false;
    private AudioSource audioSource;
    private AudioClip recordedClip;
    string trackName;
    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    } 
    public void RecordMusic(string _trackName)
    {
        if (!isRecording)
        {
            // Start recording
            Debug.Log("Start recording...");
            trackName = _trackName;
            recordedClip = Microphone.Start(null, false, 10, 44100);
            isRecording = true;
        }
    }
    public void StopRecording()
    {
        // Stop recording and save the clip
        Debug.Log("Stop recording...");
        Microphone.End(null);
        isRecording = false;

        // Save the recorded clip
        SavWav.Save(trackName, recordedClip);
        Debug.Log("Clip saved.");
    }
}
