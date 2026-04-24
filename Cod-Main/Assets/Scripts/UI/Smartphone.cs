using Runtime.Scripts.Interactables;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    [RequireComponent(typeof(UIDocument))]
    public class Smartphone : MonoBehaviour
    {
        [SerializeField] private Raycaster raycaster;

        [Header("Status Bar")]
        [SerializeField] private string time = "9:41";
        [SerializeField] private string cellularLabel = "3G";

        private UIDocument uiDocument;
        private VisualElement root;
        private Label timeLabelEl;
        private Label cellularLabelEl;
        private bool isOpen;

        private void Awake()
        {
            uiDocument = GetComponent<UIDocument>();
            root = uiDocument.rootVisualElement;
            timeLabelEl = root.Q<Label>("timeLabel");
            cellularLabelEl = root.Q<Label>("cellularLabel");
            ApplyStatusBar();
            SetVisible(false);
        }

        public void SetTime(string value)
        {
            time = value;
            if (timeLabelEl != null) timeLabelEl.text = value;
        }

        public void SetCellular(string value)
        {
            cellularLabel = value;
            if (cellularLabelEl != null) cellularLabelEl.text = value;
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

        private void ApplyStatusBar()
        {
            if (timeLabelEl != null) timeLabelEl.text = time;
            if (cellularLabelEl != null) cellularLabelEl.text = cellularLabel;
        }

        private void SetVisible(bool visible)
        {
            isOpen = visible;
            if (root == null) return;
            root.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}
