using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioMixer AudioMixer;

    public const string MixerOpciones_Master = "Master";
    public const string MixerOpciones_Music = "Música";
    public const string MixerOpciones_FX = "FX";
    public const string MixerOpciones_UI = "UI";
    public const string MixerOpciones_Ambient = "Ambient";

    public const string Key_Master = "Master";
    public const string Key_Musica = "Música";
    public const string Key_FX = "FX";
    public const string Key_UI = "UI";
    public const string Key_Ambient = "Ambient";

    public GameObject menu;
    public GameObject buttonAbrir;

    public void AbrirMenu()
    {
        menu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CerrarMenu()
    {
        menu.SetActive(false);
        Time.timeScale = 1f;
        buttonAbrir.SetActive(true);
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }


    }
    private void Start()
    {
        loadVolume();
        menu.SetActive(false);
    }

    void loadVolume()
    {
        float masterVolume = PlayerPrefs.GetFloat(Key_Master);
        float musicaVolume = PlayerPrefs.GetFloat(Key_Musica);
        float fxVolume = PlayerPrefs.GetFloat(Key_FX);
        float uiVolume = PlayerPrefs.GetFloat(Key_UI);
        float ambientVolume = PlayerPrefs.GetFloat(Key_Ambient);


        AudioMixer.SetFloat(MixerOpciones_Master, Mathf.Log10(masterVolume) * 20);
        AudioMixer.SetFloat(MixerOpciones_Music, Mathf.Log10(musicaVolume) * 20);
        AudioMixer.SetFloat(MixerOpciones_FX, Mathf.Log10(fxVolume) * 20);
        AudioMixer.SetFloat(MixerOpciones_UI, Mathf.Log10(uiVolume) * 20);
        AudioMixer.SetFloat(MixerOpciones_Ambient, Mathf.Log10(ambientVolume) * 20);


        Debug.Log(masterVolume);
        Debug.Log(musicaVolume);
        Debug.Log(fxVolume);
        Debug.Log(uiVolume);
        Debug.Log(ambientVolume);
    }
}
