using UnityEngine;
using StarterAssets;

public class PauseManager : MonoBehaviour
{
    public static bool IsPaused = false;

    [Header("UI Panels")]
    [SerializeField] private GameObject _pausePanel; // El panel de pausa
    [SerializeField] private GameObject _settingsPanel; // El panel de ajustes (si tienes uno)

    [Header("WebGL Config")]
    [SerializeField] private GameObject _quitButton;
    [SerializeField] private GameObject _graphicsSettings;

    private StarterAssetsInputs _playerInput;

    private void Start()
    {
        _playerInput = FindFirstObjectByType<StarterAssetsInputs>();

        if (_pausePanel != null) _pausePanel.SetActive(false);
        if (_settingsPanel != null) _settingsPanel.SetActive(false);

        // Lógica WebGL: Ocultar botones que no funcionan en navegador
#if UNITY_WEBGL
            if (_quitButton != null) _quitButton.SetActive(false);
            if (_graphicsSettings != null) _graphicsSettings.SetActive(false);
#endif
        Cursor.visible = false;
    }

    private void Update()
    {
        // Usamos tu método GetEscapeUI() del StarterAssetsInputs
        if (_playerInput != null && _playerInput.GetEscapeUI())
        {
            if (IsPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        IsPaused = true;
        //Time.timeScale = 0f; // Es mejor tenerlo activo para que nada se mueva atrás

        if (_pausePanel != null) _pausePanel.SetActive(true);

        // 1. Cerramos el panel de proyectos (esto ocultará el cursor por un instante)
        UIManager.Instance.ClosePanel();

        // 2. Notificamos a los sistemas (PlayerInteraction, etc.)
        UIManager.TriggerOnPanelToggled(true);

        // 3. FORZAMOS el cursor a ser visible DESPUÉS de haber cerrado lo demás
        UIManager.Instance.ForceCursorState(true);
    }

    public void Resume()
    {
        IsPaused = false;
        Time.timeScale = 1f;

        if (_pausePanel != null) _pausePanel.SetActive(false);
        if (_settingsPanel != null) _settingsPanel.SetActive(false);

        // Volvemos al estado de juego
        UIManager.TriggerOnPanelToggled(false);

        // Aseguramos que el cursor se bloquee de nuevo
        UIManager.Instance.ForceCursorState(false);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        // Cargamos el menú usando el SceneLoader que hicimos antes
        SceneLoader.Instance.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Saliendo del juego...");
    }

    public void OpenSettings()
    {
        if (_pausePanel != null) _pausePanel.SetActive(false);
        if (_settingsPanel != null) _settingsPanel.SetActive(true);

        Debug.Log("Abriendo Ajustes...");
    }

    public void CloseSettings()
    {
        if (_settingsPanel != null) _settingsPanel.SetActive(false);
        if (_pausePanel != null) _pausePanel.SetActive(true);

        Debug.Log("Volviendo al Menú de Pausa");
    }
}