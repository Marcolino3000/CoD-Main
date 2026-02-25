using Runtime.Scripts.Interactables;
using UnityEngine;
using UnityEngine.UIElements;

public class JournalMenu : MonoBehaviour
{
    [SerializeField] private Raycaster raycaster;
    [SerializeField] private bool journalIsUnlocked;

    private UIDocument uiDocument;
    private VisualElement root;

    private VisualElement startMenu;
    private VisualElement mapMenu;
    private VisualElement journalMenu;

    private VisualElement rightSideContainer;

    private VisualElement map;
    private VisualElement journal;

    private Button startButton;
    private Button resumeButton;
    private Button exitButton;


    private bool journalIsVisible;
    private bool mapIsVisible;

    private void Start()
    {
        SetupElements();
    }
    
    public void UnlockJournal()
    {
        journalIsUnlocked = true;
        rightSideContainer.style.display = DisplayStyle.Flex;
    }

    private void SetupElements()
    {
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        startMenu = root.Q("startMenu");
        rightSideContainer = root.Q("rightSideContainer");
        journalMenu = root.Q("journalMenu");
        mapMenu = root.Q("mapMenu");

        startMenu.style.display = DisplayStyle.Flex;
        rightSideContainer.style.display = DisplayStyle.None;

        SetupButtons(startMenu);
    }

    private void SetupButtons(VisualElement menu)
    {
        startButton = menu.Q<Button>("Start");
        resumeButton = menu.Q<Button>("Resume");
        exitButton = menu.Q<Button>("Exit");
        
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
        
        rightSideContainer.style.display = journalIsUnlocked ? DisplayStyle.Flex : DisplayStyle.None;
        journalMenu.style.display = journalIsUnlocked ? DisplayStyle.Flex : DisplayStyle.None;
    }

    public void ToggleMap()
    {
        mapIsVisible = !mapIsVisible;
        journalIsVisible = false;
        
        root.visible = journalIsVisible;
        raycaster.isDialogRunning = journalIsVisible;
    }

    public void ToggleJournal()
    {
        if (!journalIsUnlocked)
            return;
        
        if(journalIsVisible)
        {
            journalIsVisible = false;
            root.visible = false;
            raycaster.isDialogRunning = false;
        }

        else
        {
            journalIsVisible = true;
            root.visible = true;
            raycaster.isDialogRunning = true;
            rightSideContainer.style.display = DisplayStyle.Flex;
            journalMenu.style.display = DisplayStyle.Flex;
            mapMenu.style.display = DisplayStyle.None;   
            
            // mapMenu.style.visibility = Visibility.Hidden;
        }

        mapIsVisible = false;

    }
}