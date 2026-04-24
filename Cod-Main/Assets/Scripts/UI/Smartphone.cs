using Runtime.Scripts.Interactables;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    [RequireComponent(typeof(UIDocument))]
    public class Smartphone : MonoBehaviour
    {
        [SerializeField] private Raycaster raycaster;

        private UIDocument uiDocument;
        private VisualElement root;
        private bool isOpen;

        private void Awake()
        {
            uiDocument = GetComponent<UIDocument>();
            root = uiDocument.rootVisualElement;
            SetVisible(false);
        }

        public void Toggle()
        {
            if (isOpen) Close();
            else Open();
        }

        public void Open()
        {
            SetVisible(true);
            if (raycaster != null) raycaster.isDialogRunning = true;
        }

        public void Close()
        {
            SetVisible(false);
            if (raycaster != null) raycaster.isDialogRunning = false;
        }

        private void SetVisible(bool visible)
        {
            isOpen = visible;
            if (root == null) return;
            root.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}
