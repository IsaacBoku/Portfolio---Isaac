using UnityEngine;
using UnityEngine.Events;

public class RequiredItemInteraction : MonoBehaviour,IInteractable
{
    [Header("Requisito")]
    [SerializeField] private string requiredID;

    [Header("Eventos de Respuesta")]
    public UnityEvent OnSuccess;
    public UnityEvent OnFail;

    public void Interact()
    {
        // Comprobamos con el Singleton si tenemos el item
        if (InventoryManager.Instance.HasItem(requiredID))
        {
            // 1. Ejecutamos la acción (Abrir puerta, activar consola, etc.)
            OnSuccess?.Invoke();

            // 2. Intentamos consumirlo (el Manager ya sabe si debe borrarlo o no)
            InventoryManager.Instance.ConsumeItem(requiredID);
        }
        else
        {
            OnFail?.Invoke();
            Debug.Log("No tienes el objeto necesario.");
        }
    }

    public string GetInteractionText() => "Usar objeto necesario";
}
