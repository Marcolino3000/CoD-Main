using System;
using System.Collections;
using Runtime.Scripts.Core;
using SceneManagement;
using UnityEngine;

namespace DefaultNamespace
{
    public class TitleSequenceTrigger : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float durationBeforeTitleScreen;
        [SerializeField] private float titleScreenDuration;

        [Header("References")]
        [SerializeField] private Reaction marianneSprachiReaction;
        [SerializeField] private UnityEngine.UI.Image titleScreenImage;
        // [SerializeField] private GameObject marlene;

        private void Awake()
        {
            // marlene?.SetActive(false);
            
            marianneSprachiReaction.OnStartDialog += TogglePlayButton;
            marianneSprachiReaction.OnStopDialog += TogglePlayButton;
            marianneSprachiReaction.OnReactionFinished += OnSprachiFinished;
        }

        private void OnSprachiFinished(bool completed)
        {
            StartCoroutine(TriggerTitleScreenAndScene2());
        }

        private IEnumerator TriggerTitleScreenAndScene2()
        {
            yield return new WaitForSeconds(durationBeforeTitleScreen);
            
            while(SceneFader.Instance.IsFadingOut)
                yield return null;
            
            titleScreenImage.enabled = true;
            
            while (SceneFader.Instance.IsFadingIn)
                yield return null;
            
            yield return new WaitForSeconds(titleScreenDuration);
            
            // marlene?.SetActive(true);
            
            SceneSwapManager.ChangeScene("Scene 2");
        }

        private void TogglePlayButton()
        {
            throw new NotImplementedException();
        }
    }
}