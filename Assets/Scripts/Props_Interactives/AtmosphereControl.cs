using UnityEngine;
using System.Collections;

public class AtmosphereControl : MonoBehaviour, IInteractable
{
    [Header("Refencias de Escena")]
    [SerializeField] private Light _sunLight;

    [Header("Configuración de Día")]
    [SerializeField] private Color _dayColor = Color.white;
    [SerializeField] private float _dayIntensity = 1.0f;

    [Header("Configuración de Noche")]
    [SerializeField] private Color _nightColor = new Color(0.1f, 0.1f, 0.25f);
    [SerializeField] private float _nightIntensity = 0.2f;

    [Header("Ajustes de Transición")]
    [SerializeField] private float _transitionDuration = 1.5f;

    private bool _isNight = false;
    private Coroutine _transitionCoroutine;

    #region Implementación de IInteractable

    public string GetInteractionText()
    {
        return _isNight ? "Restaurar Iluminación (Día)" : "Simular Ambiente Nocturno";
    }

    public void Interact()
    {
        _isNight = !_isNight;

        // Detenemos cualquier transición en curso para evitar conflictos
        if (_transitionCoroutine != null) StopCoroutine(_transitionCoroutine);

        _transitionCoroutine = StartCoroutine(TransitionEnvironment(_isNight));
    }

    #endregion

    #region Lógica de Transición (Interpolación)

    private IEnumerator TransitionEnvironment(bool toNight)
    {
        float elapsed = 0;

        // Valores iniciales
        float startIntensity = _sunLight.intensity;
        Color startColor = _sunLight.color;

        // Valores objetivo
        float targetIntensity = toNight ? _nightIntensity : _dayIntensity;
        Color targetColor = toNight ? _nightColor : _dayColor;

        // Activamos niebla si vamos hacia la noche
        if (toNight) RenderSettings.fog = true;

        while (elapsed < _transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _transitionDuration;

            // Interpolación suave (SmoothStep para un look más orgánico)
            float smoothT = Mathf.SmoothStep(0, 1, t);

            _sunLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, smoothT);
            _sunLight.color = Color.Lerp(startColor, targetColor, smoothT);

            // Si la niebla está activa, ajustamos su color también
            if (RenderSettings.fog)
                RenderSettings.fogColor = _sunLight.color;

            yield return null;
        }

        // Aseguramos valores finales exactos
        _sunLight.intensity = targetIntensity;
        _sunLight.color = targetColor;

        if (!toNight) RenderSettings.fog = false;
    }

    #endregion
}
