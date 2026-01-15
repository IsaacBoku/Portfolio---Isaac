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
        Time.timeScale = 0f; // Detenemos el tiempo
        if (_pausePanel != null) _pausePanel.SetActive(true);

        // 1. Si tenías un panel de proyecto abierto, lo cerramos
        UIManager.Instance.ClosePanel();

        // 2. Usamos el método de tu UIManager para liberar el cursor
        // (Como tu método ManageCursor es privado, podemos llamar a TriggerOnPanelToggled)
        UIManager.TriggerOnPanelToggled(true);
    }

    public void Resume()
    {
        IsPaused = false;
        Time.timeScale = 1f; // Reanudamos el tiempo
        if (_pausePanel != null) _pausePanel.SetActive(false);
        if (_settingsPanel != null) _settingsPanel.SetActive(false);

        // 3. Volvemos al estado de juego (bloquea cursor y avisa al PlayerInteraction)
        UIManager.TriggerOnPanelToggled(false);
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
}