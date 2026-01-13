using UnityEngine;

public class ProjectPedestal : MonoBehaviour, IInteractable
{
    [SerializeField] private ProjectData _projectData;

    // Al usar la interfaz, el texto de interacción es dinámico
    public string GetInteractionText()
    {
        return _projectData != null ? $"Explorar {_projectData.projectName}" : "Sin datos de proyecto";
    }

    public void Interact()
    {
        if (_projectData == null)
        {
            Debug.LogWarning("Este pedestal no tiene asignado un ProjectData.");
            return;
        }

        // Enviamos el objeto de datos completo al UIManager
        UIManager.Instance.DisplayProjectInfo(_projectData);
    }
}