using UnityEngine;
using UnityEngine.Rendering;

public class VisualStyleSwitcher : MonoBehaviour, IInteractable
{
    [Header("Referencias de Sistema")]
    [SerializeField] private Volume _globalVolume;
    [SerializeField] private GameObject _pixelateEffectObject; // El objeto con el shader de pixelado

    [Header("Perfiles de Volumen")]
    [SerializeField] private VolumeProfile _modernProfile;
    [SerializeField] private VolumeProfile _retroProfile;

    private bool _isRetro = false;

    public string GetInteractionText()
    {
        return _isRetro ? "Estilo: Moderno" : "Estilo: Retro-Pixel";
    }

    public void Interact()
    {
        if (_globalVolume == null) return;

        _isRetro = !_isRetro;

        // 1. Cambiamos el perfil (Color, Grano, Contraste)
        _globalVolume.profile = _isRetro ? _retroProfile : _modernProfile;

        // 2. Activamos/Desactivamos el efecto de pixelado
        if (_pixelateEffectObject != null)
        {
            _pixelateEffectObject.SetActive(_isRetro);
        }

        Debug.Log("Cambio de estilo visual ejecutado. ¿Retro?: " + _isRetro);
    }
}
