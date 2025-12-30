using UnityEngine;

public class GravityController : MonoBehaviour, IInteractable
{
    [Header("Configuración de Física")]
    [SerializeField] private Vector3 _normalGravity = new Vector3(0, -9.81f, 0);
    [SerializeField] private Vector3 _lowGravity = new Vector3(0, -1.5f, 0);

    private bool _isLowGravity = false;

    // CORRECCIÓN: Usamos _isLowGravity en lugar de _isNight
    public string GetInteractionText()
    {
        return _isLowGravity ? "Restaurar Gravedad Normal" : "Activar Gravedad Lunar";
    }

    public void Interact()
    {
        _isLowGravity = !_isLowGravity;

        // Cambiamos la gravedad global del motor de física
        Physics.gravity = _isLowGravity ? _lowGravity : _normalGravity;

        Debug.Log("Gravedad cambiada. ¿Es baja?: " + _isLowGravity);
    }
}
