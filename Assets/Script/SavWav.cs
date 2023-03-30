using UnityEngine;
using System.Collections;
using System.IO;
using System;

public static class SavWav
{
    const int HEADER_SIZE = 44;
    // This method saves an AudioClip to a WAV file.
    // The method takes a filename and an AudioClip as input parameters.
    // The AudioClip is converted to a float array, which is then used to create the WAV file.
    // The method returns true if the file was saved successfully, false otherwise.
    public static bool Save(string filename, AudioClip clip)
    {
        Debug.Log("filename " + filename);
        Debug.Log("clip" + clip);
        if (!filename.ToLower().EndsWith(".wav"))
        {
            filename += ".wav";
        }

        var filepath = Path.Combine(Application.streamingAssetsPath + "/Tracks/", filename);

        Debug.Log("Saving clip to " + filepath);

        // Make sure the directory exists
        Directory.CreateDirectory(Path.GetDirectoryName(filepath));

        // Convert the audio clip to a float array
        var samples = new float[clip.samples];
        clip.GetData(samples, 0);

        // Create an empty file for the WAV file and write the header.
        using (var fileStream = CreateEmpty(filepath))
        {
            ConvertAndWrite(fileStream, samples);
            WriteHeader(fileStream, clip);
        }

        return true; 
    }
    // This method creates an empty WAV file and writes the header to the file.
    // The method takes a filepath as an input parameter and returns a FileStream object.

    private static FileStream CreateEmpty(string filepath)
    {
        var fileStream = new FileStream(filepath, FileMode.Create);
        // Write an empty header to the file.
        byte emptyByte = new byte();

        for (int i = 0; i < HEADER_SIZE; i++) // Preparing the header
        {
            fileStream.WriteByte(emptyByte);
        }

        return fileStream;
    }
    // This method converts a float array to an Int16 array and writes the data to the file.
    // The method takes a FileStream object and a float array as input parameters.

    private static void ConvertAndWrite(FileStream fileStream, float[] samples)
    {
        Int16[] intData = new Int16[samples.Length]; // Convert the float array to an Int16 array.
         
        int rescaleFactor = 32767; // 16-bit (short) range
        for (int i = 0; i < samples.Length; i++)
        {
            intData[i] = (short)(samples[i] * rescaleFactor);
        }

        // Write the data to the file.
        Byte[] byteData = new Byte[intData.Length * 2];
        Buffer.BlockCopy(intData, 0, byteData, 0, byteData.Length);
        fileStream.Write(byteData, 0, byteData.Length);
    }
    // This method writes the header to the WAV file.
    // The method takes a FileStream object and an AudioClip as input parameters.


    /*This method takes in a FileStream and an AudioClip object. It writes the header information to the file stream,
     * which includes the RIFF chunk descriptor, RIFF stands for "Resource Interchange File Format."   
     * The RIFF format consists of a header followed by data chunks.*/
    private static void WriteHeader(FileStream fileStream, AudioClip clip)
    {
        // Get the sample rate, number of channels, and number of samples from the AudioClip.
        var hz = clip.frequency;
        var channels = clip.channels;
        var samples = clip.samples;
        // Move to the beginning of the file.
        fileStream.Seek(0, SeekOrigin.Begin);

        Byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
        fileStream.Write(riff, 0, 4);

        Byte[] chunkSize = BitConverter.GetBytes(fileStream.Length - 8);
        fileStream.Write(chunkSize, 0, 4);

        Byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
        fileStream.Write(wave, 0, 4);

        Byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
        fileStream.Write(fmt, 0, 4);

        Byte[] subChunk1 = BitConverter.GetBytes(16);
        fileStream.Write(subChunk1, 0, 4);

        UInt16 two = 2;
        UInt16 one = 1;

        Byte[] audioFormat = BitConverter.GetBytes(one);
        fileStream.Write(audioFormat, 0, 2);

        Byte[] numChannels = BitConverter.GetBytes(channels);
        fileStream.Write(numChannels, 0, 2);

        Byte[] sampleRate = BitConverter.GetBytes(hz);
        fileStream.Write(sampleRate, 0, 4);

        Byte[] byteRate = BitConverter.GetBytes(hz * channels * 2); // SampleRate * BytesPer
        fileStream.Write(byteRate, 0, 4);

        UInt16 blockAlign = (ushort)(channels * 2);
        fileStream.Write(BitConverter.GetBytes(blockAlign), 0, 2);

        UInt16 bps = 16;
        Byte[] bitsPerSample = BitConverter.GetBytes(bps);
        fileStream.Write(bitsPerSample, 0, 2);

        Byte[] datastring = System.Text.Encoding.UTF8.GetBytes("data");
        fileStream.Write(datastring, 0, 4);

        Byte[] subChunk2 = BitConverter.GetBytes(samples * channels * 2);
        fileStream.Write(subChunk2, 0, 4);

        Debug.Log("WAV file saved.");
    }
}
