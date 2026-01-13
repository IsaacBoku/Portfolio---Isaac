using UnityEngine;

[CreateAssetMenu(fileName = "Nuevo Proyecto", menuName = "Portfolio/Proyecto")]
public class ProjectData : ScriptableObject
{
    [Header("Información Básica")]
    public string projectName;
    [TextArea(5, 10)] public string projectDescription;

    [Header("Multimedia")]
    public Sprite projectScreenshot; // La imagen que quieres mostrar
    public string itchIoUrl;

    [Header("Metadatos Técnicos")]
    public string techStack; // Ej: "Unity, C#, ShaderGraph"
    public string role;      // Ej: "Lead Programmer"

    public bool isImageNative;
}