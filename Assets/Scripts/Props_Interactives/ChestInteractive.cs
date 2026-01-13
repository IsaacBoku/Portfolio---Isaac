using UnityEngine;

public class ChestInteractive : MonoBehaviour,IInteractable
{
    [SerializeField] private bool isOpen;

    public void Interact()
    {
        if (!isOpen)
        {
            OpenChest();
        }
        else
        {
            CloseChest();
        }
    }
    public string GetInteractionText()
    {
        return isOpen ? "Cerrar Cofre" : "Abrir Cofre";
    }
    private void OpenChest()
    {
        // Lógica para abrir el cofre (animación, efectos, etc.)
        Debug.Log("Cofre abierto.");
        isOpen = true;
    }
    private void CloseChest()
    {
        // Lógica para cerrar el cofre (animación, efectos, etc.)
        Debug.Log("Cofre cerrado.");
        isOpen = false;
    }

}
