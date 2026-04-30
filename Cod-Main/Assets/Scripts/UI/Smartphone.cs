using System;
using System.Collections.Generic;
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

        [Header("Chats")]
        [SerializeField] private TextAsset contactsJson;
        [SerializeField] private VisualTreeAsset chatCardTemplate;

        private UIDocument uiDocument;
        private VisualElement root;
        private Label timeLabelEl;
        private Label cellularLabelEl;
        private ListView chatListView;
        private readonly List<Contact> contacts = new();
        private bool isOpen;

        private void Start()
        {
            uiDocument = GetComponent<UIDocument>();
            LoadContacts();
            BindRoot(initial: true);
        }

        private void Update()
        {
            // UI Toolkit Live Reload rebuilds the panel tree whenever the
            // source UXML/USS reimports during Play mode. The cached refs
            // become stale and the new ListView has no makeItem/bindItem.
            // Detect via reference change and re-bind.
            if (uiDocument == null) return;
            var current = uiDocument.rootVisualElement;
            if (current != null && current != root)
            {
                BindRoot(initial: false);
            }
        }

        private void BindRoot(bool initial)
        {
            root = uiDocument.rootVisualElement;
            if (root == null)
            {
                if (initial)
                    Debug.LogError("Smartphone: UIDocument.rootVisualElement is null. Is the source asset assigned?", this);
                return;
            }

            timeLabelEl = root.Q<Label>("timeLabel");
            cellularLabelEl = root.Q<Label>("cellularLabel");
            chatListView = root.Q<ListView>("chatListView");

            ApplyStatusBar();
            BindChatList();
            SetVisible(initial ? false : isOpen);
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
            // ListView in a hidden panel has a 0x0 viewport and won't
            // create rows. Refresh once visible so cards appear.
            chatListView?.RefreshItems();
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

        private void LoadContacts()
        {
            contacts.Clear();
            if (contactsJson == null)
            {
                Debug.LogWarning("[Smartphone] 'Contacts Json' field is not assigned. Drag Assets/UI/Smartphone/Contacts.json onto it in the Inspector.", this);
                return;
            }
            var parsed = JsonUtility.FromJson<ContactsRoot>(contactsJson.text);
            if (parsed?.contacts == null || parsed.contacts.Count == 0)
            {
                Debug.LogWarning($"[Smartphone] JSON parsed but produced 0 contacts (text length = {contactsJson.text.Length}). Check field names match (contact, unreadCount, messages, text, timestamp).", this);
                return;
            }
            contacts.AddRange(parsed.contacts);
            Debug.Log($"[Smartphone] Loaded {contacts.Count} contacts.", this);
        }

        private void BindChatList()
        {
            if (chatListView == null)
            {
                Debug.LogWarning("[Smartphone] No element named 'chatListView' found in the UXML.", this);
                return;
            }
            if (chatCardTemplate == null)
            {
                Debug.LogWarning("[Smartphone] 'Chat Card Template' field is not assigned. Drag Assets/UI/Smartphone/ChatCard.uxml onto it in the Inspector.", this);
                return;
            }

            // Set makers BEFORE itemsSource — assigning itemsSource is the
            // trigger that begins binding, and it needs makeItem ready.
            chatListView.makeItem = () =>
            {
                var card = chatCardTemplate.Instantiate();
                card.userData = new ChatCardRefs
                {
                    Name = card.Q<Label>("cardName"),
                    Preview = card.Q<Label>("cardPreview"),
                    Time = card.Q<Label>("cardTime"),
                    Badge = card.Q<VisualElement>("cardBadge"),
                    BadgeLabel = card.Q<Label>("cardBadgeLabel"),
                };
                return card;
            };

            chatListView.bindItem = (element, index) =>
            {
                var refs = element.userData as ChatCardRefs;
                if (refs == null) return;
                var c = contacts[index];

                refs.Name.text = c.contact;

                var (preview, timeStr) = LastMessage(c);
                refs.Preview.text = preview;
                refs.Time.text = timeStr;

                if (c.unreadCount > 0)
                {
                    refs.Badge.style.display = DisplayStyle.Flex;
                    refs.BadgeLabel.text = c.unreadCount.ToString();
                }
                else
                {
                    refs.Badge.style.display = DisplayStyle.None;
                }
            };

            chatListView.itemsSource = contacts;
            chatListView.Rebuild();
            Debug.Log($"[Smartphone] Chat list bound with {contacts.Count} rows.", this);
        }

        private static (string text, string time) LastMessage(Contact c)
        {
            if (c.messages == null || c.messages.Count == 0) return (string.Empty, string.Empty);
            var last = c.messages[c.messages.Count - 1];
            var timeStr = string.Empty;
            if (DateTime.TryParse(last.timestamp, out var dt))
                timeStr = dt.ToString("HH:mm");
            var preview = last.text?.Replace("\n", " ").Trim() ?? string.Empty;
            return (preview, timeStr);
        }

        private class ChatCardRefs
        {
            public Label Name;
            public Label Preview;
            public Label Time;
            public VisualElement Badge;
            public Label BadgeLabel;
        }

        [Serializable]
        private class ContactsRoot
        {
            public List<Contact> contacts;
        }

        [Serializable]
        private class Contact
        {
            public string contact;
            public int unreadCount;
            public List<ContactMessage> messages;
        }

        [Serializable]
        private class ContactMessage
        {
            public string text;
            public string timestamp;
        }
    }
}
