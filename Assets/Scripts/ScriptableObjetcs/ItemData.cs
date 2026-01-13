using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("Identificación")]
    public string itemID; // ID único (ej: "llave_roja", "fusible_01")
    public string itemName;

    [Header("Visual")]
    public Sprite icon;
    [TextArea] public string description;

    public bool isConsumable;
}
