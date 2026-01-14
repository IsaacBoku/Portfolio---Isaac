using StarterAssets;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private CanvasGroup _projectPanelGroup; // Usamos CanvasGroup para efectos
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _infoText;
    [SerializeField] private Button _linkButton;
    [SerializeField] private Image _projectImageHolder;


    // Eventos para que el juego sepa cuándo la UI está activa
    public static event Action<bool> OnPanelToggled;

    private string _currentURL;

    private StarterAssetsInputs playerInput;

    private void Awake()
    {
        // Singleton robusto
        if (Instance == null)
        {
            Instance = this;
            TogglePanel(false); // Estado inicial
        }
        else
        {
            Destroy(gameObject);
        }

        playerInput = FindFirstObjectByType<StarterAssetsInputs>();
    }

    private void Update()
    {
        // UX: Permitir cerrar con la tecla Escape
        if (_projectPanelGroup.alpha > 0 && playerInput.GetEscapeUI())
        {
            ClosePanel();
        }
    }

    public void DisplayProjectInfo(string title, string info, string url)
    {
        _titleText.text = title;
        _infoText.text = info;
        _currentURL = url;

        


        // Si no hay URL, ocultamos el botón de enlace para evitar errores
        _linkButton.gameObject.SetActive(!string.IsNullOrEmpty(url));

        TogglePanel(true);
    }

    public void OpenLink()
    {
        if (!string.IsNullOrEmpty(_currentURL))
        {
            Application.OpenURL(_currentURL);
            ClosePanel();
        }
    }

    public void ClosePanel()
    {
        TogglePanel(false);

        Cursor.visible = false;
    }

    public void DisplayProjectInfo(ProjectData data)
    {
        _projectPanelGroup.alpha = 1;
        _titleText.text = data.projectName;
        _projectPanelGroup.interactable = true;
        _projectPanelGroup.blocksRaycasts = true;

        // Formateamos el texto para que incluya los stats técnicos antes de la descripción
        _infoText.text = $"<b>TECH:</b> {data.techStack}\n<b>ROLE:</b> {data.role}\n\n{data.projectDescription}";

        _currentURL = data.itchIoUrl;

        // Gestión de la imagen
        if (_projectImageHolder != null)
        {

            _projectImageHolder.sprite = data.projectScreenshot;
            // Si no hay imagen, ocultamos el componente para que no se vea un cuadro blanco
            _projectImageHolder.enabled = data.projectScreenshot != null;
            if (data.isImageNative)
            {
                // Si la imagen es nativa, ajustamos el tamaño automáticamente
                _projectImageHolder.SetNativeSize();
            }
            else
            {
                _projectImageHolder.rectTransform.sizeDelta = new Vector2(600, 800); // Tamaño por defecto
            }
        }

        _linkButton.gameObject.SetActive(!string.IsNullOrEmpty(_currentURL));

        ManageCursor(true);
        OnPanelToggled?.Invoke(true);
    }
    private void TogglePanel(bool isActive)
    {
        // En lugar de SetActive, usamos Alpha e Interactable para permitir transiciones
        _projectPanelGroup.alpha = isActive ? 1 : 0;
        _projectPanelGroup.interactable = isActive;
        _projectPanelGroup.blocksRaycasts = isActive;

        // Gestión del cursor centralizada
        ManageCursor(isActive);

        // Notificamos a otros sistemas (ej: para pausar el movimiento del jugador)
        OnPanelToggled?.Invoke(isActive);
    }

    private void ManageCursor(bool show)
    {
        Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = show;
    }
}
