using System;
using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(menuName = "Settings/InGameAudioSettings")]
    public class InGameAudioSettings : ScriptableObject
    {
        [Range(0f, 1f)]
        [SerializeField] private float masterVolume = 1f;
        
        [Range(0f, 1f)]
        [SerializeField] private float musicVolume = 1f;
        
        [Range(0f, 1f)]
        [SerializeField] private float sfxVolume = 1f;
        
        [Range(0f, 1f)]
        [SerializeField] private float dialogVolume = 1f;

        private void OnValidate()
        {
            UpdateWwiseRTPCs();
        }

        private void OnEnable()
        {
            UpdateWwiseRTPCs();
        }

        public void SetMasterVolume(float value)
        {
            masterVolume = value;
            UpdateWwiseRTPCs();
        }

        public void SetMusicVolume(float value)
        {
            musicVolume = value;
            UpdateWwiseRTPCs();
        }

        public void SetSfxVolume(float value)
        {
            sfxVolume = value;
            UpdateWwiseRTPCs();
        }

        public void SetDialogVolume(float value)
        {
            dialogVolume = value;
        }

        private void UpdateWwiseRTPCs()
        {
            AkUnitySoundEngine.SetRTPCValue("VOL_Master", masterVolume);
            AkUnitySoundEngine.SetRTPCValue("VOL_Music", musicVolume);
            AkUnitySoundEngine.SetRTPCValue("VOL_SFX", sfxVolume);
        }
    }
}