using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject iconPrefab;
    [SerializeField] private Transform container;
    [SerializeField] private GameObject inventoryPanel;

    [Header("Texto de Nombre")]
    [SerializeField] private TextMeshProUGUI itemNameText; // Arrastra aquí el nuevo texto
    [SerializeField] private float textDisplayTime = 2f;   // Cuánto tarda en desaparecer

    [Header("Selección Visual")]
    [SerializeField] private RectTransform selectionHighlight;
    [SerializeField] private float smoothSpeed = 15f; 

    private class InventorySlot
    {
        public ItemData data;
        public GameObject iconObj;
    }

    private List<InventorySlot> _slots = new List<InventorySlot>();
    private StarterAssetsInputs playerInput;
    private int _selectedIndex = 0;
    private float _scrollCooldown = 0.15f;
    private float _nextScrollTime;

    private Coroutine _moveRoutine; 
    private Coroutine _textFadeRoutine;

    private void Start()
    {
        playerInput = FindFirstObjectByType<StarterAssetsInputs>();
        if (inventoryPanel != null) inventoryPanel.SetActive(false);
        if (selectionHighlight != null) selectionHighlight.gameObject.SetActive(false);

        // Empezamos con el texto invisible
        if (itemNameText != null) itemNameText.alpha = 0;
    }

    private void Update()
    {
        if (playerInput != null && playerInput.GetScrollWeel(out float scrollValue))
        {
            if (Time.time >= _nextScrollTime && _slots.Count > 1)
            {
                int direction = scrollValue > 0 ? -1 : 1;
                ChangeSelection(direction);
                _nextScrollTime = Time.time + _scrollCooldown;
            }
        }
    }

    private void ChangeSelection(int direction)
    {
        _selectedIndex = (_selectedIndex + direction + _slots.Count) % _slots.Count;
        UpdateVisualSelection();

        // Lógica de nombre: mostramos el nombre cada vez que cambiamos
        ShowItemName(_slots[_selectedIndex].data.itemName);
    }

    private void ShowItemName(string name)
    {
        if (itemNameText == null) return;

        itemNameText.text = name;

        if (_textFadeRoutine != null) StopCoroutine(_textFadeRoutine);
        _textFadeRoutine = StartCoroutine(FadeTextRoutine());
    }

    private IEnumerator FadeTextRoutine()
    {
        // Aparece instantáneamente (o podrías hacerle un fade in también)
        itemNameText.alpha = 1;

        // Espera el tiempo configurado
        yield return new WaitForSeconds(textDisplayTime);

        // Desvanecimiento suave (Fade Out)
        float elapsed = 0;
        float fadeDuration = 0.5f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            itemNameText.alpha = Mathf.Lerp(1, 0, elapsed / fadeDuration);
            yield return null;
        }

        itemNameText.alpha = 0;
        _textFadeRoutine = null;
    }

    private void UpdateVisualSelection()
    {
        if (_slots.Count == 0 || selectionHighlight == null)
        {
            if (selectionHighlight != null) selectionHighlight.gameObject.SetActive(false);
            if (itemNameText != null) itemNameText.alpha = 0;
            return;
        }

        selectionHighlight.gameObject.SetActive(true);
        if (_moveRoutine != null) StopCoroutine(_moveRoutine);
        _moveRoutine = StartCoroutine(MoveSelectionRoutine(_slots[_selectedIndex].iconObj.transform));
    }

    // CORRUTINA: Mueve el selector suavemente hacia el objetivo
    private IEnumerator MoveSelectionRoutine(Transform target)
    {
        // Aseguramos que el selector esté en el mismo nivel jerárquico que el contenedor para moverse libremente
        selectionHighlight.SetParent(inventoryPanel.transform);

        Vector3 targetPos = target.position;

        // Mientras no estemos lo suficientemente cerca del objetivo
        while (Vector3.Distance(selectionHighlight.position, target.position) > 0.1f)
        {
            // Interpolar posición (Lerp)
            selectionHighlight.position = Vector3.Lerp(selectionHighlight.position, target.position, Time.deltaTime * smoothSpeed);
            yield return null;
        }

        // Al terminar, nos pegamos exactamente al objetivo y lo hacemos hijo para que lo siga si el panel se mueve
        selectionHighlight.position = target.position;
        selectionHighlight.SetParent(target);
        selectionHighlight.SetAsLastSibling();
        _moveRoutine = null;
    }

    public ItemData GetSelectedItem() => _slots.Count > 0 ? _slots[_selectedIndex].data : null;

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
        if (inventoryPanel != null) inventoryPanel.SetActive(true);

        GameObject newIcon = Instantiate(iconPrefab, container);
        newIcon.GetComponentInChildren<Image>().sprite = item.icon;

        _slots.Add(new InventorySlot { data = item, iconObj = newIcon });

        if (_slots.Count == 1)
        {
            _selectedIndex = 0;
            selectionHighlight.gameObject.SetActive(true);
            selectionHighlight.SetParent(newIcon.transform);
            selectionHighlight.localPosition = Vector3.zero;
            ShowItemName(item.itemName); // <--- Mostrar nombre al recoger el primero
        }
    }

    private void RemoveIcon(ItemData item)
    {
        InventorySlot slotToRemove = _slots.Find(s => s.data == item);

        if (slotToRemove != null)
        {
            // Salvaguarda: si el selector es hijo del que vamos a borrar, lo sacamos
            if (selectionHighlight != null && selectionHighlight.IsChildOf(slotToRemove.iconObj.transform))
            {
                selectionHighlight.SetParent(inventoryPanel.transform);
                selectionHighlight.gameObject.SetActive(false);
            }

            Destroy(slotToRemove.iconObj);
            _slots.Remove(slotToRemove);

            if (_slots.Count > 0)
            {
                _selectedIndex = Mathf.Clamp(_selectedIndex, 0, _slots.Count - 1);
                UpdateVisualSelection();
            }
            else
            {
                if (selectionHighlight != null) selectionHighlight.gameObject.SetActive(false);
                if (inventoryPanel != null) inventoryPanel.SetActive(false);
            }
        }
    }
}