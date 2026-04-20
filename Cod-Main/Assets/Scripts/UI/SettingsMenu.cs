using Audio;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] InGameAudioSettings audioSettings;
        
        private Slider masterVolume;
        private Slider dialogVolume;
        private Slider musicVolume;
        private Slider sfxVolume;
        
        private UIDocument uiDocument;
        private VisualElement root;

        private void Start()
        {
            GetElements();
            SetupEvents();
        }

        private void GetElements()
        {
            uiDocument = GetComponent<UIDocument>();
            root = uiDocument.rootVisualElement;
            
            masterVolume = root.Q<Slider>("masterVolume");
            dialogVolume = root.Q<Slider>("dialogVolume");
            musicVolume = root.Q<Slider>("musicVolume");
            sfxVolume = root.Q<Slider>("sfxVolume");
        }

        private void SetupEvents()
        {
            masterVolume.RegisterValueChangedCallback(
                evt =>
                {
                    audioSettings.SetMasterVolume(evt.newValue);
                });
            
            dialogVolume.RegisterValueChangedCallback(
                evt =>
                {
                    audioSettings.SetDialogVolume(evt.newValue);
                });
            
            musicVolume.RegisterValueChangedCallback(
                evt =>
                {
                    audioSettings.SetMusicVolume(evt.newValue);
                });
            
            sfxVolume.RegisterValueChangedCallback(
                evt =>
                {
                    audioSettings.SetSfxVolume(evt.newValue);
                });
        }
    }
}