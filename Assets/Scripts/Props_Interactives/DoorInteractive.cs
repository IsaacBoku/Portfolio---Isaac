using System.Collections;
using UnityEngine;

public class DoorInteractive : MonoBehaviour
{
    [Header("Door Lift Settings")]
    [SerializeField] private bool isOpen;
    [SerializeField] private float liftHeight = 3f; // Cuántas unidades sube
    [SerializeField] private float duration = 1.0f; // Tiempo que tarda en subir

    private bool isMoving;
    private Vector3 closedPosition;
    private Vector3 openedPosition;

    private void Start()
    {
        // Guardamos la posición inicial como la "cerrada"
        closedPosition = transform.localPosition;
        // Calculamos la posición "abierta" sumando la altura al eje Y
        openedPosition = closedPosition + new Vector3(0, liftHeight, 0);
    }

    // Método para ser llamado desde tu sistema de interacción
    public void ToggleDoor()
    {
        if (isMoving) return; // Evita que se solapen movimientos

        if (!isOpen)
            OpenDoor();
        else
            CloseDoor();
    }

    public void OpenDoor()
    {
        isOpen = true;
        StartCoroutine(AnimateDoor(openedPosition));
    }

    public void CloseDoor()
    {
        isOpen = false;
        StartCoroutine(AnimateDoor(closedPosition));
    }

    private IEnumerator AnimateDoor(Vector3 targetPosition)
    {
        isMoving = true;
        Vector3 startPosition = transform.localPosition;
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / duration;

            // Usamos SmoothStep para que el movimiento no sea brusco (aceleración/deceleración suave)
            t = Mathf.SmoothStep(0f, 1f, t);

            // Lerp = Linear Interpolation para posiciones
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);

            yield return null;
        }

        transform.localPosition = targetPosition;
        isMoving = false;

        Debug.Log(isOpen ? "Puerta subida (Abierta)." : "Puerta bajada (Cerrada).");
    }
}
