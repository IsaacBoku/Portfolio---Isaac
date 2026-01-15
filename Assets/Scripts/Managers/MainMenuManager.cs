using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject mainButtonsPanel;

    [Header("WebGL Controls")]
    [SerializeField] private GameObject quitButton;
    [SerializeField] private GameObject graphicsSettings;

    private void Start()
    {
        CheckPlatform();
        settingsPanel.SetActive(false);
    }

    private void CheckPlatform()
    {
        // Si estamos en WebGL, ocultamos lo que no sirve
#if UNITY_WEBGL
            if (quitButton != null) quitButton.SetActive(false);
            if (graphicsSettings != null) graphicsSettings.SetActive(false);
            Debug.Log("Detectado WebGL: Botones innecesarios ocultados.");
#endif
    }

    public void PlayGame()
    {
        SceneLoader.Instance.LoadScene("Scene_Hub"); // Pon el nombre de tu escena
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        mainButtonsPanel.SetActive(false);
    }

    public void BackToMenu()
    {
        settingsPanel.SetActive(false);
        mainButtonsPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Saliendo del juego...");
    }
}
