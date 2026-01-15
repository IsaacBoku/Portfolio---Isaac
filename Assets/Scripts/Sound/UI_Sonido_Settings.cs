using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_Sonido_Settings : MonoBehaviour
{
    public AudioMixer audioMixer;

    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider fxSlider;
    [SerializeField] private Slider uiSlider;
    [SerializeField] private Slider ambientSlider;

    const string MixerOpciones_Master = "Master";
    const string MixerOpciones_Music = "Música";
    const string MixerOpciones_FX = "FX";
    const string MixerOpciones_UI = "UI";
    const string MixerOpciones_Ambient = "Ambient";

    private void Awake()
    {
        masterSlider.onValueChanged.AddListener(setMasterVolume);
        musicSlider.onValueChanged.AddListener(setMusicVolume);
        fxSlider.onValueChanged.AddListener(setFXVolume);
        uiSlider.onValueChanged.AddListener(setUIVolume);
        ambientSlider.onValueChanged.AddListener(setAmbientVolume);
    }

    public void setMasterVolume(float volume)
    {
        audioMixer.SetFloat(MixerOpciones_Master, Mathf.Log10(volume) * 20);
    }
    public void setMusicVolume(float volume)
    {
        audioMixer.SetFloat(MixerOpciones_Music, Mathf.Log10(volume) * 20);
    }

    public void setFXVolume(float volume)
    {
        audioMixer.SetFloat(MixerOpciones_FX, Mathf.Log10(volume) * 20);
    }
    private void setUIVolume(float volume)
    {
        audioMixer.SetFloat(MixerOpciones_UI, Mathf.Log10(volume) * 20);
    }
    private void setAmbientVolume(float volume)
    {
        audioMixer.SetFloat(MixerOpciones_Ambient, Mathf.Log10(volume) * 20);
    }
    private void OnEnable()
    {
        masterSlider.value = PlayerPrefs.GetFloat(SoundManager.Key_Master);
        musicSlider.value = PlayerPrefs.GetFloat(SoundManager.Key_Musica);
        fxSlider.value = PlayerPrefs.GetFloat(SoundManager.Key_FX);
        uiSlider.value = PlayerPrefs.GetFloat(SoundManager.Key_UI);
        ambientSlider.value = PlayerPrefs.GetFloat(SoundManager.Key_Ambient);
    }

    private void OnDisable()
    {
        SaveSonido();
    }

    public void SaveSonido()
    {
        PlayerPrefs.SetFloat(SoundManager.Key_Master, masterSlider.value);
        PlayerPrefs.SetFloat(SoundManager.Key_Musica, musicSlider.value);
        PlayerPrefs.SetFloat(SoundManager.Key_FX, fxSlider.value);
        PlayerPrefs.SetFloat(SoundManager.Key_UI, uiSlider.value);
        PlayerPrefs.SetFloat(SoundManager.Key_Ambient, ambientSlider.value);

        Debug.Log("Se ha guardado");
    }
}
