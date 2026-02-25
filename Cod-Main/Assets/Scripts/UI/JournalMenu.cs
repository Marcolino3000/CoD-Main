using Runtime.Scripts.Interactables;
using UnityEngine;
using UnityEngine.UIElements;

public class JournalMenu : MonoBehaviour
{
    [SerializeField] private Raycaster raycaster;
    
    [SerializeField] private bool gameStarted;
    
    private UIDocument uiDocument;
    private VisualElement root;
    
    private void Start()
    {
        SetupElements();
        ShowMenu();
    }

    private void SetupElements()
    {
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
    }

    private void StartGame()
    {
        HideMenu();
    }

    private void ResumeGame()
    {
        HideMenu();
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void HideMenu()
    {
        root.visible = false;
        raycaster.isDialogRunning = false;
    }

    public void ShowMenu()
    {
        root.visible = true;
        raycaster.isDialogRunning = true;
    }
}