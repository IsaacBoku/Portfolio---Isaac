using UnityEngine;
using TMPro; // Asegúrate de tener TextMeshPro importado si usas texto
using System.Collections; // Para las Corrutinas

[RequireComponent(typeof(CanvasGroup))] // Nos aseguramos de que siempre haya un CanvasGroup
public class HologramController : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("El componente CanvasGroup del Canvas para controlar la transparencia.")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [Tooltip("El TextMeshProUGUI si el holograma es un texto.")]
    [SerializeField] private TextMeshProUGUI _hologramText; // Opcional, si es solo imagen puedes borrarlo

    [Header("Efecto Flotante")]
    [Tooltip("Velocidad de rotación del holograma.")]
    [SerializeField] private float _rotationSpeed = 30f;
    [Tooltip("Amplitud del movimiento vertical (cuánto sube/baja).")]
    [SerializeField] private float _floatAmplitude = 0.1f;
    [Tooltip("Frecuencia del movimiento vertical (qué tan rápido sube/baja).")]
    [SerializeField] private float _floatFrequency = 0.5f;

    [Header("Efecto Parpadeo (Flicker)")]
    [Tooltip("Habilita o deshabilita el efecto de parpadeo.")]
    [SerializeField] private bool _useFlicker = true;
    [Tooltip("Probabilidad (0-1) de que el holograma parpadee en un frame dado.")]
    [Range(0f, 1f)]
    [SerializeField] private float _flickerChance = 0.02f; // 2% de probabilidad cada frame
    [Tooltip("Duración mínima del parpadeo.")]
    [SerializeField] private float _minFlickerDuration = 0.05f;
    [Tooltip("Duración máxima del parpadeo.")]
    [SerializeField] private float _maxFlickerDuration = 0.15f;
    [Tooltip("Alfa mínimo durante el parpadeo.")]
    [Range(0f, 1f)]
    [SerializeField] private float _minFlickerAlpha = 0.3f;

    [Header("Opciones de Visibilidad")]
    [Tooltip("Tiempo en segundos que tarda en aparecer/desaparecer.")]
    [SerializeField] private float _fadeDuration = 0.5f;

    private Vector3 _startPosition;
    private Coroutine _flickerCoroutine; // Para controlar la corrutina de parpadeo
    private Coroutine _fadeCoroutine;    // Para controlar la corrutina de fade

    private void Awake()
    {
        // Nos aseguramos de tener el CanvasGroup, si no existe lo añadimos
        if (_canvasGroup == null)
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
            {
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }
        // Aseguramos que el holograma esté invisible al inicio
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false; // No debe bloquear clics
    }

    private void Start()
    {
        _startPosition = transform.position;
        // Al iniciar, el holograma debería aparecer suavemente
        ShowHologram(true);
    }

    private void Update()
    {
        // Movimiento flotante
        transform.position = new Vector3(
            _startPosition.x,
            _startPosition.y + Mathf.Sin(Time.time * _floatFrequency) * _floatAmplitude,
            _startPosition.z
        );

        // Rotación
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);

        // Parpadeo
        if (_useFlicker && Random.value < _flickerChance && _flickerCoroutine == null)
        {
            _flickerCoroutine = StartCoroutine(DoFlicker());
        }
    }

    /// <summary>
    /// Activa o desactiva la visibilidad del holograma con un fade suave.
    /// </summary>
    /// <param name="show">True para mostrar, False para ocultar.</param>
    public void ShowHologram(bool show)
    {
        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(FadeHologram(show));
    }

    private IEnumerator FadeHologram(bool fadeIn)
    {
        float startAlpha = _canvasGroup.alpha;
        float targetAlpha = fadeIn ? 1f : 0f;
        float timeElapsed = 0f;

        while (timeElapsed < _fadeDuration)
        {
            _canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timeElapsed / _fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        _canvasGroup.alpha = targetAlpha; // Aseguramos que termine en el valor exacto

        _canvasGroup.blocksRaycasts = fadeIn; // Bloquea si está visible
        _canvasGroup.interactable = fadeIn; // Intercepta clics si está visible
    }

    private IEnumerator DoFlicker()
    {
        // Guardamos el alfa actual para restaurarlo
        float originalAlpha = _canvasGroup.alpha;

        _canvasGroup.alpha = Random.Range(_minFlickerAlpha, originalAlpha); // Parpadea a un alfa menor
        yield return new WaitForSeconds(Random.Range(_minFlickerDuration, _maxFlickerDuration));
        _canvasGroup.alpha = originalAlpha; // Vuelve al alfa original

        _flickerCoroutine = null; // Liberamos la corrutina
    }

    // Opcional: Para cambiar el texto del holograma
    public void SetText(string newText)
    {
        if (_hologramText != null)
        {
            _hologramText.text = newText;
        }
    }
}