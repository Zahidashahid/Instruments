using System.IO;
using UnityEngine;

public class DeleteSong : MonoBehaviour
{
    // This script deletes a saved audio file from the device's persistent data path
    // This method deletes the audio file with the given name
    // The name of the audio file should include the file extension, e.g. ".wav"
    public void DeleteTrack(string audioFileNme)
    {
        if (!audioFileNme.ToLower().EndsWith(".wav"))
        {
            // Check if the file name ends with ".wav", if not, add it
            audioFileNme += ".wav";
        }
        // Combine the file path with the persistent data path of the application
        string filePath = Path.Combine(Application.persistentDataPath, audioFileNme);

        if (File.Exists(filePath)) // Check if the file exists at the given path, if so, delete it
        {
            File.Delete(filePath);
            Debug.Log("Song deleted successfully.");
        }
        else
        {
            Debug.LogError("Song file not found.");
        }
    }
}
