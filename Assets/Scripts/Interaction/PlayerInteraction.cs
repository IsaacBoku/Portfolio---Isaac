using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Configuración de Raycast")]
    public float _interactRange = 3f;
    public LayerMask interactableLayer;
    [SerializeField] private Transform _camTransform;

    [Header("Referencias UI")]
    public TextMeshProUGUI promptText;
    [SerializeField] private Image cursorDot; // El puntito del centro

    private StarterAssetsInputs playerInputHandler;
    private bool _isUIActive = false;

    private void Start()
    {
        playerInputHandler = GetComponentInParent<StarterAssetsInputs>();
        if (promptText != null) promptText.text = "";
    }

    private void OnEnable() => UIManager.OnPanelToggled += HandleUIState;
    private void OnDisable() => UIManager.OnPanelToggled -= HandleUIState;

    private void HandleUIState(bool isActive)
    {
        _isUIActive = isActive;

        // Si la UI está activa (Inventario, Pausa), ocultamos todo
        if (_isUIActive)
        {
            promptText.text = "";
            if (cursorDot != null) cursorDot.gameObject.SetActive(false);
        }
        else
        {
            // Al cerrar UI, volvemos a mostrar el punto por defecto
            if (cursorDot != null) cursorDot.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (_isUIActive) return;

        Ray ray = new Ray(_camTransform.position, _camTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _interactRange, interactableLayer))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                // LÓGICA PEDIDA: Quitamos el punto y ponemos el texto
                UpdateCursorVisibility(false);
                promptText.text = "[E] " + interactable.GetInteractionText();

                if (playerInputHandler.GetInteraction())
                {
                    interactable.Interact();
                }
            }
            else
            {
                ResetInteraction();
            }
        }
        else
        {
            ResetInteraction();
        }
    }

    private void UpdateCursorVisibility(bool isVisible)
    {
        if (cursorDot == null) return;

        // Solo actuamos si el estado actual es diferente al deseado (optimización)
        if (cursorDot.gameObject.activeSelf != isVisible)
        {
            cursorDot.gameObject.SetActive(isVisible);
        }
    }

    private void ResetInteraction()
    {
        promptText.text = "";
        // Si no estamos mirando nada interactuable, el punto vuelve a aparecer
        UpdateCursorVisibility(true);
    }
}