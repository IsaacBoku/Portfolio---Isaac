using UnityEngine;
using UnityEngine.Events;

public class ItemPickUp : MonoBehaviour,IInteractable
{
    [SerializeField] private ItemData itemToGive;

    public void Interact()
    {
        InventoryManager inv = FindFirstObjectByType<InventoryManager>();

        if (inv != null)
        {
            inv.AddItem(itemToGive);
            Debug.Log($"Recogiste: {itemToGive.itemName}");
            Destroy(gameObject); // El objeto físico desaparece
        }
    }

    public string GetInteractionText()
    {
        return $"Recoger {itemToGive.itemName}";
    }
}
