using Runtime.Scripts.Interactables;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Raycaster raycaster;
    
    private Button startButton;
    private Button resumeButton;
    private Button exitButton;
    
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
        
        startButton = root.Q<Button>("Start");
        resumeButton = root.Q<Button>("Resume");
        exitButton = root.Q<Button>("Exit");
        
        startButton.clicked += StartGame;
        resumeButton.clicked += ResumeGame;
        exitButton.clicked += ExitGame;
    }

    private void StartGame()
    {
        HideMenu();
        
        startButton.SetEnabled(false);
        startButton.pickingMode = PickingMode.Ignore;
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
