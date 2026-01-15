using UnityEngine;
using UnityEngine.Events;
using System.Collections; // Necesario para la Corrutina

public class RequiredItemInteraction : MonoBehaviour, IInteractable
{
    [Header("Requisito")]
    [SerializeField] private string requiredID;
    [SerializeField] private string itemNameToShow; // Nombre "bonito" del item para mostrar al jugador

    [Header("Eventos de Respuesta")]
    public UnityEvent OnSuccess;
    public UnityEvent OnFail;

    private bool _isDisplayingError = false;

    public void Interact()
    {
        InventoryUI ui = FindFirstObjectByType<InventoryUI>();
        ItemData selectedItem = ui != null ? ui.GetSelectedItem() : null;

        if (selectedItem != null && selectedItem.itemID == requiredID)
        {
            Debug.Log($"Éxito: {requiredID} usado correctamente.");
            OnSuccess?.Invoke();
            InventoryManager.Instance.ConsumeItem(requiredID);
        }
        else
        {
            OnFail?.Invoke();
            // Iniciamos el efecto visual de error
            StartCoroutine(FlashErrorText());

            string mensaje = (selectedItem == null) ? "No tienes nada seleccionado." : $"El objeto {selectedItem.itemName} no sirve aquí.";
            Debug.Log(mensaje);
        }
    }

    public string GetInteractionText()
    {
        // Si estamos en medio de un error, el texto cambia (opcional, si quieres reforzar el feedback)
        if (_isDisplayingError)
        {
            return $"<color=red>¡Ese objeto no sirve!</color>";
        }

        // Texto dinámico que indica qué se necesita
        return $"Necesitas: <color=#FFD700>{itemNameToShow}</color>";
    }

    private IEnumerator FlashErrorText()
    {
        if (_isDisplayingError) yield break;

        _isDisplayingError = true;

        // Buscamos el texto de la UI para cambiarle el color directamente (opcional)
        // Pero como GetInteractionText() ya cambia según _isDisplayingError,
        // solo necesitamos esperar un momento y refrescar.

        yield return new WaitForSeconds(1.5f); // Duración del mensaje de error

        _isDisplayingError = false;
    }
}