using Nodes;
using Nodes.Decorator;
using Tree;
using UnityEngine;
using UnityEngine.Audio;

namespace DefaultNamespace
{
    public class AudioClipPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioMixer mixer;
        [SerializeField] private AudioClip paulTalksThroughDoorClip;
        
        private void Update()
        {
            CheckForPassedMarkers();
        }
        
        private void CheckForPassedMarkers()
        {
            if (audioSource.clip == null || !audioSource.isPlaying)
                return;
            
            // if(audioSource.timeSamples > marker.samples)
        }

        public void PlayClip(Node node)
        {
            if(node.AudioClip == null) 
                return;
            
            var defaultSnapshot = mixer.FindSnapshot("Default");
            defaultSnapshot.TransitionTo(0f);

            if(node.AudioClip == paulTalksThroughDoorClip)
            {
                var snapshot = mixer.FindSnapshot("Lowpass");
                if (snapshot != null)
                {
                    snapshot.TransitionTo(0f);
                }
                else
                {
                    Debug.LogWarning("Snapshot " + snapshot.name + " not found");
                }
            }
            
            audioSource.volume = node is not PlayerDialogOption ? 0.4f : 1f;
            
            audioSource.clip = node.AudioClip;
            
            // get markers
            
            audioSource.Play();
        }
        
        
        private void Awake()
        {
            DialogTreeRunner.DialogNodeSelected += PlayClip;
        }
    }
}