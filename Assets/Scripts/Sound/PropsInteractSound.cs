using UnityEngine;
using UnityEngine.Audio;

public class PropsInteractSound : MonoBehaviour
{
    public SoundsScriptable[] sonidosSettings;

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioMixer audioMixer;

    public void playSoundSFX(string name)
    {
        AudioClip clipToPlay = null;

        foreach (var setting in sonidosSettings)
        {
            foreach (var sonido in setting.arraySonidos)
            {
                if (sonido.soundName == name)
                {
                    clipToPlay = sonido.clip;
                    break;
                }
            }
            if (clipToPlay != null)
                break;
        }

        if (clipToPlay != null)
        {
            audioSource.clip = clipToPlay;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning($"No se encontr? el sonido con nombre: {name}");
        }
    }
}
