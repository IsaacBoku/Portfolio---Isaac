using UnityEngine;
using System.Collections;

public class AtmosphereControl : MonoBehaviour
{
    [Header("Referencias de Escena")]
    [SerializeField] private Light _sunLight;
    [SerializeField] private Material _skyboxMaterial; // El material del Skybox (Exposure será clave)

    [Header("Configuración de Día")]
    [SerializeField] private Color _dayColor = Color.white;
    [SerializeField] private float _dayIntensity = 1.0f;
    [SerializeField] private float _daySkyExposure = 1.0f;
    [SerializeField] private float _dayAmbientIntensity = 1.0f;

    [Header("Configuración de Noche")]
    [SerializeField] private Color _nightColor = new Color(0.1f, 0.1f, 0.25f);
    [SerializeField] private float _nightIntensity = 0.1f;
    [SerializeField] private float _nightSkyExposure = 0.15f;
    [SerializeField] private float _nightAmbientIntensity = 0.2f;

    [Header("Ajustes de Transición")]
    [SerializeField] private float _transitionDuration = 2.0f;
    [SerializeField] private AnimationCurve _transitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private bool _isNight = false;
    private Coroutine _transitionCoroutine;

    private void Start()
    {
        // Aseguramos que el Skybox sea una instancia única para no editar el archivo original
        if (RenderSettings.skybox != null)
            RenderSettings.skybox = new Material(RenderSettings.skybox);
    }

    #region Implementación de IInteractable

    public string GetInteractionText()
    {
        return _isNight ? "Restaurar Iluminación (Día)" : "Simular Ambiente Nocturno";
    }

    public void Interact()
    {
        _isNight = !_isNight;

        if (_transitionCoroutine != null) StopCoroutine(_transitionCoroutine);
        _transitionCoroutine = StartCoroutine(TransitionEnvironment(_isNight));
    }

    #endregion

    private IEnumerator TransitionEnvironment(bool toNight)
    {
        float elapsed = 0;

        // Valores iniciales
        float startIntensity = _sunLight.intensity;
        Color startColor = _sunLight.color;
        float startSkyExp = RenderSettings.skybox.GetFloat("_Exposure");
        float startAmbient = RenderSettings.ambientIntensity;

        // Valores objetivo
        float targetIntensity = toNight ? _nightIntensity : _dayIntensity;
        Color targetColor = toNight ? _nightColor : _dayColor;
        float targetSkyExp = toNight ? _nightSkyExposure : _daySkyExposure;
        float targetAmbient = toNight ? _nightAmbientIntensity : _dayAmbientIntensity;

        if (toNight) RenderSettings.fog = true;

        while (elapsed < _transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _transitionDuration;
            float curveT = _transitionCurve.Evaluate(t); // Mucho más control que SmoothStep

            // Luz Direccional
            _sunLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, curveT);
            _sunLight.color = Color.Lerp(startColor, targetColor, curveT);

            // Skybox y Ambiente (Vital para el look nocturno)
            RenderSettings.skybox.SetFloat("_Exposure", Mathf.Lerp(startSkyExp, targetSkyExp, curveT));
            RenderSettings.ambientIntensity = Mathf.Lerp(startAmbient, targetAmbient, curveT);

            // Niebla
            if (RenderSettings.fog)
            {
                RenderSettings.fogColor = Color.Lerp(startColor, targetColor, curveT);
                RenderSettings.fogDensity = Mathf.Lerp(0.01f, 0.05f, curveT);
            }

            // Forzamos a Unity a actualizar las reflexiones en tiempo real (Opcional, consume recursos)
            // DynamicGI.UpdateEnvironment(); 

            yield return null;
        }

        if (!toNight) RenderSettings.fog = false;
    }
}