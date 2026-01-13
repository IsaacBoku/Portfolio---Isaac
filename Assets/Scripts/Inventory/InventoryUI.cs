using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject iconPrefab;
    [SerializeField] private Transform container;

    // Diccionario para saber qué GameObject de la UI pertenece a cada ItemData
    private Dictionary<ItemData, GameObject> _uiIcons = new Dictionary<ItemData, GameObject>();

    private void OnEnable()
    {
        InventoryManager.OnItemAdded += AddIcon;
        InventoryManager.OnItemRemoved += RemoveIcon;
    }

    private void OnDisable()
    {
        InventoryManager.OnItemAdded -= AddIcon;
        InventoryManager.OnItemRemoved -= RemoveIcon;
    }

    private void AddIcon(ItemData item)
    {
        GameObject newIcon = Instantiate(iconPrefab, container);
        newIcon.GetComponent<Image>().sprite = item.icon;
        _uiIcons.Add(item, newIcon); // Guardamos la referencia
    }

    private void RemoveIcon(ItemData item)
    {
        if (_uiIcons.ContainsKey(item))
        {
            Destroy(_uiIcons[item]); // Borramos el objeto visual
            _uiIcons.Remove(item);   // Lo sacamos del diccionario
        }
    }
}
