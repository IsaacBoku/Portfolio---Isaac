using UnityEngine;
using UnityEngine.Events;

public class RequiredItemInteraction : MonoBehaviour,IInteractable
{
    [Header("Requisito")]
    [SerializeField] private string requiredID; // El ID del item que debe estar seleccionado

    [Header("Eventos de Respuesta")]
    public UnityEvent OnSuccess;
    public UnityEvent OnFail;

    public void Interact()
    {
        // 1. Buscamos el InventoryUI para saber qué hay seleccionado
        // (Si usas un Singleton tipo UIManager.Instance.inventoryUI, cámbialo aquí)
        InventoryUI ui = FindFirstObjectByType<InventoryUI>();
        ItemData selectedItem = ui != null ? ui.GetSelectedItem() : null;

        // 2. Comprobamos si el item seleccionado existe y si su ID coincide
        if (selectedItem != null && selectedItem.itemID == requiredID)
        {
            Debug.Log($"Éxito: {requiredID} usado correctamente.");

            // Ejecutamos la acción (Abrir puerta, etc.)
            OnSuccess?.Invoke();

            // Consumimos el item del inventario real
            InventoryManager.Instance.ConsumeItem(requiredID);
        }
        else
        {
            // Si no tiene nada seleccionado o el ID no coincide
            OnFail?.Invoke();

            string mensaje = (selectedItem == null) ? "No tienes nada seleccionado." : $"El objeto {selectedItem.itemName} no sirve aquí.";
            Debug.Log(mensaje);
        }
    }

    public string GetInteractionText() => "Usar objeto seleccionado";
}
