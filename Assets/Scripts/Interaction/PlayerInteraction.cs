using StarterAssets;
using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float _interactRange = 3f;
    public TextMeshProUGUI promptText;
    public LayerMask interactableLayer;
    [SerializeField] private Transform _camTransform;

    private StarterAssetsInputs playerInputHandler;
    private bool _isUIActive = false; // Nueva variable de control

    private void Start()
    {
        playerInputHandler = GetComponentInParent<StarterAssetsInputs>();
        if (promptText != null) promptText.text = "";
    }

    // 1. Nos suscribimos al evento del UIManager
    private void OnEnable()
    {
        UIManager.OnPanelToggled += HandleUIState;
    }

    private void OnDisable()
    {
        UIManager.OnPanelToggled -= HandleUIState;
    }

    // 2. Método que reacciona al cambio de estado de la UI
    private void HandleUIState(bool isActive)
    {
        _isUIActive = isActive;

        // Si la UI se abre, limpiamos el texto inmediatamente
        if (_isUIActive)
        {
            promptText.text = "";
        }
    }

    void Update()
    {
        // 3. Si la UI está activa, no procesamos ni el Raycast ni el Input
        if (_isUIActive) return;

        Ray ray = new Ray(_camTransform.position, _camTransform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * _interactRange, Color.red);

        if (Physics.Raycast(ray, out hit, _interactRange, interactableLayer))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                promptText.text = "[E] " + interactable.GetInteractionText();

                if (playerInputHandler.GetInteraction())
                {
                    interactable.Interact();
                }
            }
            else
            {
                promptText.text = "";
            }
        }
        else
        {
            promptText.text = "";
        }
    }
}
