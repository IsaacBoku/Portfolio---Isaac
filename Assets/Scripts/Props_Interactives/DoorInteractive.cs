using System.Collections;
using UnityEngine;

public class DoorInteractive : MonoBehaviour, IInteractable
{
    [Header("Door Settings")]
    [SerializeField] private bool isOpen;
    [SerializeField] private float openAngle;
    [SerializeField] private float duration;

    private bool isRotating;

    private void Start()
    {
        openAngle = 90f;
        duration = 0.5f;
    }
    public void Interact()
    {
        if(isRotating) return;

        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }
    public string GetInteractionText()
    {
        if(isRotating) return "...";
        return isOpen ? "Close Door" : "Open Door";
    }

    private void OpenDoor()
    {
        isOpen = true;

        // Quaternion.Euler convierte grados (x,y,z) a la matemática compleja de Unity (Cuaterniones)
        Quaternion targetRotation  = Quaternion.Euler(0, openAngle, 0) * transform.localRotation;

        // Iniciamos la Corutina (el proceso en segundo plano)
        StartCoroutine(AnimateDoor(targetRotation));

    }

    private void CloseDoor()
    {
        isOpen = false;
        // Para cerrar, simplemente volvemos a la rotación original o restamos
        // Una forma segura es revertir el ángulo
        Quaternion targetRotation = Quaternion.Euler(0, -openAngle, 0) * transform.localRotation;

        StartCoroutine(AnimateDoor(targetRotation));
    }


    private IEnumerator AnimateDoor(Quaternion targetRotation)
    {
        isRotating = true; // Bloqueamos interacción

        Quaternion startRotation = transform.localRotation;
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            // Movemos el tiempo hacia adelante
            timeElapsed += Time.deltaTime;

            // Calculamos el porcentaje completado (de 0 a 1)
            float t = timeElapsed / duration;

            // Slerp = Spherical Linear Interpolation (Interpolación esférica)
            // Es la forma correcta de rotar suavemente entre dos puntos
            transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);

            yield return null; // Esperamos al siguiente frame
        }

        // Aseguramos que al final quede exacto (para corregir decimales pequeños)
        transform.localRotation = targetRotation;

        isRotating = false; // Desbloqueamos interacción
        Debug.Log(isOpen ? "Puerta abierta completamente." : "Puerta cerrada completamente.");
    }
}
