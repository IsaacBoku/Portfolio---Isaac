using UnityEngine;
using UnityEngine.Audio;

public enum SoundActions
{
    Steps,
    Hits,
    UI,
    Hurt
}

public class FXSoundManager : MonoBehaviour
{
    public static FXSoundManager instance;

    public GameObject soundPrefab;

    public AudioMixer audioMixer;
    public AudioMixerGroup audioMixerGroup;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySFX(AudioClip clip, Transform pos, SoundActions action)
    {
        var prefabSpawn = Instantiate(soundPrefab, pos.position, Quaternion.identity);

        var clipSpawn = prefabSpawn.GetComponent<AudioSource>();

        clipSpawn.clip = clip;
        clipSpawn.outputAudioMixerGroup = audioMixerGroup;
        clipSpawn.Play();

        Destroy(prefabSpawn, clip.length);
    }
}
