using UnityEngine;

[System.Serializable]
public class SoundSettings
{
    public string soundName;
    public AudioClip clip;
    public bool randomPitch;
    public float pitchMin = 0.85f;
    public float pitchMax = 1.2f;
}