using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // Singleton: Permite llamar a InventoryManager.Instance desde cualquier sitio
    public static InventoryManager Instance { get; private set; }

    private List<ItemData> _items = new List<ItemData>();

    // Eventos para que la UI se entere de los cambios
    public static event Action<ItemData> OnItemAdded;
    public static event Action<ItemData> OnItemRemoved;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddItem(ItemData item)
    {
        _items.Add(item);
        OnItemAdded?.Invoke(item);
    }

    public bool HasItem(string id) => _items.Exists(i => i.itemID == id);

    public void ConsumeItem(string id)
    {
        // Buscamos el item en la lista por su ID
        ItemData item = _items.Find(i => i.itemID == id);

        if (item != null && item.isConsumable)
        {
            _items.Remove(item);
            OnItemRemoved?.Invoke(item); // Avisamos a la UI que lo borre
            Debug.Log($"{item.itemName} ha sido consumido.");
        }
    }
}
