using System;
using System.Collections;
using Runtime.Scripts.Core;
using SceneManagement;
using UnityEngine;

namespace DefaultNamespace
{
    public class TitelSequenceTrigger : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float durationBeforeTitleScreen;
        [SerializeField] private float titleScreenDuration;

        [Header("References")]
        [SerializeField] private Reaction marianneSprachiReaction;
        [SerializeField] private SceneFader sceneFader;
        [SerializeField] private SceneSwapManager sceneSwapManager;
        [SerializeField] private UnityEngine.UI.Image titleScreenImage;

        private void Awake()
        {
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
            
            sceneSwapManager.ChangeScene("Scene 2");
        }

        private void TogglePlayButton()
        {
            throw new NotImplementedException();
        }
    }
}