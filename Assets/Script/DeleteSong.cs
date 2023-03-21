using System.IO;
using UnityEngine;

public class DeleteSong : MonoBehaviour
{
    public void DeleteTrack(string audioFileNme)
    {
        if (!audioFileNme.ToLower().EndsWith(".wav"))
        {
            audioFileNme += ".wav";
        }
        string filePath = Path.Combine(Application.persistentDataPath, audioFileNme);

        if (File.Exists(filePath))
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
