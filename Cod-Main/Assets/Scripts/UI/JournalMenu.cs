using System;
using Runtime.Scripts.Interactables;
using UnityEngine;
using UnityEngine.UIElements;

public class JournalMenu : MonoBehaviour
{
    public static event Action<bool> OnMenuToggled;
    
    [SerializeField] private Raycaster raycaster;
    [SerializeField] private Toggleable journalState;
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
        ShowMenu();
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
        OnMenuToggled?.Invoke(false);
        
        root.visible = false;
        raycaster.isDialogRunning = false;
    }

    public void ShowMenu()
    {
        OnMenuToggled?.Invoke(true);
        
        root.visible = true;
        raycaster.isDialogRunning = true;
        
        rightSideContainer.style.display = journalState.ToggleState ? DisplayStyle.Flex : DisplayStyle.None;
    }

    public void ToggleMap()
    {
        if (!journalState.ToggleState)
            return;
        
        if(mapIsVisible)
        {
            mapIsVisible = false;
            HideMenu();
        }

        else
        {
            mapIsVisible = true;
            ShowMenu();
            journalMenu.style.display = DisplayStyle.None;
            mapMenu.style.display = DisplayStyle.Flex;   
        }

        journalIsVisible = false;

    }

    public void ToggleJournal()
    {
        if (!journalState.ToggleState)
            return;
        
        if(journalIsVisible)
        {
            journalIsVisible = false;
            HideMenu();
        }

        else
        {
            journalIsVisible = true;
            ShowMenu();
            journalMenu.style.display = DisplayStyle.Flex;
            mapMenu.style.display = DisplayStyle.None;   
        }

        mapIsVisible = false;

    }
}