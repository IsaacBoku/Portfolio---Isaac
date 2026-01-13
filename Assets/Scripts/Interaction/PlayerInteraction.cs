using StarterAssets;
using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float _interactRange = 3f;
    public TextMeshProUGUI promptText;
    public LayerMask interactableLayer; // Filtro para solo detectar objetos en esta capa
    [SerializeField] private Transform _camTransform;

    private StarterAssetsInputs playerInputHandler;
    private Camera mainCamera;

    private void Start()
    {
        // Buscamos el componente en el objeto raíz del jugador si no está en la cámara
        playerInputHandler = GetComponentInParent<StarterAssetsInputs>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Lanzamos el rayo desde el centro de la cámara hacia adelante
        Ray ray = new Ray(_camTransform.position, _camTransform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * _interactRange, Color.red);

        // El rayo solo chocará con objetos dentro de 'interactableLayer'
        if (Physics.Raycast(ray, out hit, _interactRange, interactableLayer))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                promptText.text = "[E] " + interactable.GetInteractionText();

                // Usamos el sistema de inputs de Starter Assets
                if (playerInputHandler.GetInteraction())
                {
                    interactable.Interact();
                }
            }
        }
        else
        {
            promptText.text = "";
        }
    }
}
