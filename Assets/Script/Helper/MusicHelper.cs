using UnityEngine;

public static class MusicHelper
{
    public static AudioClip BytesToAudioClip(byte[] audioBytes, int sampleRate, int channels)
    {
        float[] samples = BytesToFloatAudio(audioBytes);

        // Create AudioClip if null
        AudioClip audioClip = AudioClip.Create("AudioClip", samples.Length, channels, sampleRate, false);
        audioClip.SetData(samples, 0);
        return audioClip;
    }

    public static float[] BytesToFloatAudio(byte[] audioBytes)
    {
        // Convert byte array to float array
        float[] samples = new float[audioBytes.Length / 2]; // 2 bytes per sample
        for (int i = 0; i < samples.Length; i++)
        {
            short val = (short)((audioBytes[i * 2 + 1] << 8) | audioBytes[i * 2]);
            samples[i] = val / 32768f; // normalize to [-1, 1]
        }

        return samples;
    }
}