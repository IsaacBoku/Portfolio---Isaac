using UnityEngine;

public class ProjectPedestal : MonoBehaviour, IInteractable
{
    public string projectName;
    [TextArea] public string projectDescription;
    public string itchIoUrl;

    public string GetInteractionText() => "Explorar " + projectName;

    public void Interact()
    {
        UIManager.Instance.DisplayProjectInfo(projectName, projectDescription, itchIoUrl);
    }
}
