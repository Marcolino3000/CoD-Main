using Runtime.Scripts.Interactables;
using UnityEngine;
using UnityEngine.UIElements;

public class JournalMenu : MonoBehaviour
{
    [SerializeField] private Raycaster raycaster;
    
    [SerializeField] private bool gameStarted;
    
    private UIDocument uiDocument;
    private VisualElement root;
    private bool isVisible;
    
    private void Start()
    {
        SetupElements();
    }

    private void SetupElements()
    {
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
    }

    public void ToggleMenu()
    {
        isVisible = !isVisible;
        
        root.visible = isVisible;
        raycaster.isDialogRunning = isVisible;
    }
}