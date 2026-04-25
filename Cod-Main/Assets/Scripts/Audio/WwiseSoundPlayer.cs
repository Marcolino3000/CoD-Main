using Runtime.Scripts.Interactables;
using UnityEngine;

namespace Audio
{
    public class WwiseSoundPlayer : MonoBehaviour
    {
        [Header("Wwise Events")]
        [SerializeField] private AK.Wwise.Event clickEvent;
        
        [Header("References")]
        [SerializeField] private Raycaster raycaster;

        public void Play()
        {
            clickEvent.Post(gameObject);
        }
    }
}