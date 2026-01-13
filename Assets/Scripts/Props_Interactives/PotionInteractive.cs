using UnityEngine;

public class PotionInteractive : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Poción bebida.");
        Destroy(gameObject);
    }
    public string GetInteractionText()
    {
        return "Beber Poción";
    }
}
