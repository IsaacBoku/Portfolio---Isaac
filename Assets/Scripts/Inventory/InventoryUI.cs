using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject iconPrefab;
    [SerializeField] private Transform container;

    // IMPORTANTE: Este debe ser un objeto HIJO del objeto que tiene este script
    [SerializeField] private GameObject inventoryPanel;

    private Dictionary<ItemData, GameObject> _uiIcons = new Dictionary<ItemData, GameObject>();

    private void Start()
    {
        // Al usar Start, nos aseguramos de que el Manager ya esté listo
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
        }
    }

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
        // 1. Activamos el panel visual
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(true);
        }

        // 2. Instanciamos el icono
        if (iconPrefab != null && container != null)
        {
            GameObject newIcon = Instantiate(iconPrefab, container);

            // Buscamos la imagen (ya sea en el objeto o en sus hijos)
            Image img = newIcon.GetComponentInChildren<Image>();
            if (img != null) img.sprite = item.icon;

            _uiIcons.Add(item, newIcon);
        }
    }

    private void RemoveIcon(ItemData item)
    {
        if (_uiIcons.ContainsKey(item))
        {
            Destroy(_uiIcons[item]);
            _uiIcons.Remove(item);
        }
    }
}
