using System.Collections;
using UnityEngine;

public class InstrumentRecorder : MonoBehaviour
{
    // Audio recording parameters
    public int frequency = 44100;
    public int sampleLengthSeconds = 5;

    // Audio clip to store the recorded audio
    private AudioClip recordedClip;

    // Start recording when the script is enabled
    private void OnEnable()
    {
        recordedClip = Microphone.Start(null, false, sampleLengthSeconds, frequency);
    }

    // Stop recording and save the recorded clip when the script is disabled
    private void OnDisable()
    {
        Microphone.End(null);
        SaveClip();
    }

    // Save the recorded clip as a WAV file
    private void SaveClip()
    {
        if (recordedClip == null)
        {
            Debug.LogWarning("Recorded clip is null.");
            return;
        }

        string filePath = Application.dataPath + "/recorded_clip.wav";
        SavWav.Save(filePath, recordedClip);
        Debug.Log("Clip saved to " + filePath);
    }
}