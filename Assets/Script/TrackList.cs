using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrackList : MonoBehaviour
{
    public GameObject[] trackItems;
    public Button playButton;
    public Button pauseButton;
    public Button deleteButton;
    public PlayAudioFromFile playAudioFromFile;
    private List<AudioClip> tracks ;
    private int currentTrackIndex = -1;
    private AudioSource audioSource;
    private void Start()
    {
        //FileChecking();
    }
    private void OnEnable()
    {
        tracks = new List<AudioClip>();
        Debug.Log("--------------GameObject enabled:---------------- " + gameObject.name);
        FileChecking();
        
    }
    private void FileChecking()
    {
        // Load all audio files in the "Tracks" folder
        string tracksPath =  Application.streamingAssetsPath + "/Tracks/";
        DirectoryInfo tracksDir = new DirectoryInfo(tracksPath);
        FileInfo[] trackFiles = tracksDir.GetFiles("*.wav");
        
        Debug.Log("-----trackFiles------- " + trackFiles.Length);

            Debug.Log("tracks.Count before " + tracks.Count);
        for (int i = 0; i < trackFiles.Length; i++)
        {
            Debug.Log("audioClip name " + trackFiles[i]);
            string trackFilePath = tracksPath + trackFiles[i].Name;
            WWW trackLoader = new WWW("file://" + trackFilePath);
            while (!trackLoader.isDone) { }

            AudioClip track = trackLoader.GetAudioClip();
            track.name = Path.GetFileNameWithoutExtension(trackFiles[i].Name);
            Debug.Log("tracks.Count added " + tracks.Count);
            tracks.Add(track);
        } 
            Debug.Log("tracks.Count now " + tracks.Count);
        // Hide excess track items
        for (int i = tracks.Count; i < trackItems.Length; i++)
        {
            Debug.Log("Hide excess track items " + i);
            trackItems[i].SetActive(false);
        } 
        // Assign tracks to track items
        for (int i = 0; i < tracks.Count; i++)
        {
            trackItems[i].SetActive(true);
            audioSource = trackItems[i].GetComponent<AudioSource>();
            TMP_Text trackTitle = trackItems[i].transform.Find("Title").GetComponent<TMP_Text>();
            trackTitle.text = tracks[i].name;
            Button playButton = trackItems[i].transform.Find("PlayButton").GetComponent<Button>();
            playButton.onClick.AddListener(() => PlayTrack(trackTitle.text));
            Button pauseButton = trackItems[i].transform.Find("PauseButton").GetComponent<Button>();
            pauseButton.onClick.AddListener(() => PauseTrack());
            Button deleteButton = trackItems[i].transform.Find("DeleteButton").GetComponent<Button>();
            deleteButton.onClick.AddListener(() => DeleteTrack(trackTitle.text, i));
            if (trackFiles.Length > 0)
            { 
                // Load the audio clip from the file path
                AudioClip audioClip = LoadAudioClip(tracksPath, trackFiles[i].Name);
                // Set the audio clip to the audio source
                audioSource.clip = audioClip;
                Debug.Log("audioClip name " + audioClip.name);
            }
        }

        // Get audio source component from first track item
        
    }
    private AudioClip LoadAudioClip(string path, string audioFileNme)
    {
        // Load the audio clip from the file path using WWW class
        path = Path.Combine(Application.streamingAssetsPath + "/Tracks/", audioFileNme);
       
        if (!path.ToLower().EndsWith(".wav"))
        {
            path += ".wav";
        }
        WWW audioLoader = new WWW("file://" + path); //WW is used for unity web request 
        // Wait until the audio clip is completely loaded
        Debug.Log("path " + path);
        Debug.Log("audioSource.name " + audioLoader);

        while (!audioLoader.isDone) { }
        // Return the loaded audio clip
        return audioLoader.GetAudioClip(false, false);
    }
    private void PlayTrack(string filename)
    {
        playAudioFromFile.PlayTrack(filename);
    }

    private void PauseTrack()
    {
        audioSource.Pause();
    }

    private void DeleteTrack(string filename,int  index)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath + "/Tracks/", filename);
          

        if (!filePath.ToLower().EndsWith(".wav"))
        {
            filePath += ".wav";
        }

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
       // FileChecking();
    }
}
