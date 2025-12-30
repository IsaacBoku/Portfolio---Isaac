using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Panel de Proyecto")]
    public GameObject projectPanel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI infoText;
    public Button linkButton;

    private string currentURL;

    void Awake() { Instance = this; projectPanel.SetActive(false); }

    public void DisplayProjectInfo(string title, string info, string url)
    {
        projectPanel.SetActive(true);
        titleText.text = title;
        infoText.text = info;
        currentURL = url;

        // Bloquear cursor para interactuar con el ratón en el panel
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OpenLink() => Application.OpenURL(currentURL);

    public void ClosePanel()
    {
        projectPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
