using UnityEngine;

public class CoinRotate : MonoBehaviour
{
    [Header("Ajustes de Giro")]
    [SerializeField] private float _rotationSpeed = 100f;
    [SerializeField] private Vector3 _rotationDirection = Vector3.up;

    [Header("Ajustes de Flotado (Opcional)")]
    [SerializeField] private bool _enableBobbing = true;
    [SerializeField] private float _bobIntensity = 0.5f;
    [SerializeField] private float _bobSpeed = 2f;

    private Vector3 _startPosition;

    void Start()
    {
        // Guardamos la posición inicial para el efecto de flotado
        _startPosition = transform.position;
    }

    void Update()
    {
        // 1. Giro constante
        // Multiplicamos por Time.deltaTime para que el giro sea fluido independientemente de los FPS
        transform.Rotate(_rotationDirection * _rotationSpeed * Time.deltaTime);

        // 2. Efecto de flotado (Seno)
        if (_enableBobbing)
        {
            float newY = _startPosition.y + Mathf.Sin(Time.time * _bobSpeed) * _bobIntensity;
            transform.position = new Vector3(_startPosition.x, newY, _startPosition.z);
        }
    }
}