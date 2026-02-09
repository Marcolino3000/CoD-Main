using Editor.AudioEditor;
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
        [SerializeField] private MarkerManager markerManager;
        

        private void Update()
        {
            if (!audioSource.isPlaying) 
                return;
            
            var playheadSample = audioSource.timeSamples;
            markerManager.CheckPlayhead(audioSource.clip, playheadSample);
        }

        private void PlayClip(Node node)
        {
            if(node.AudioClip == null) 
                return;
            
            markerManager?.ResetPlayheadCheck();

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
            
            audioSource.volume = node is PlayerDialogOption ? 0.25f : 0.1f;
            audioSource.volume *= node.ClipVolume;
            
            audioSource.clip = node.AudioClip;
            
            audioSource.Play();
        }
        
        
        private void Awake()
        {
            DialogTreeRunner.DialogNodeSelected += PlayClip;
            markerManager.OnMarkerReached += OnMarkerReached;
        }

        private void OnMarkerReached(MarkerType markerType)
        {
            Debug.Log($"Clip marker reached: {markerType}");
        }
    }
}