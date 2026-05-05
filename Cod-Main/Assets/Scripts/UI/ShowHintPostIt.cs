using System.Collections;
using Runtime.Scripts.Core;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ShowHintPostIt : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float showDuration;
        [SerializeField] private float animationDuration;
        [SerializeField] private string hintText;
        [SerializeField] private RectTransform logRectTransform;
        
        [Header("References")]
        [SerializeField] private Reaction reaction;
        [SerializeField] private TextMeshProUGUI text;

        private Vector2 originalAnchoredPosition;
        private Vector2 offscreenAnchoredPosition;
        private bool isLogVisible;

        private void Start()
        {
            reaction.OnReactionFinished += OnReactionFinished;
            Setup();
        }

        private void Setup()
        {
            if (logRectTransform == null)
                // logRectTransform = logImage.GetComponent<RectTransform>();
            originalAnchoredPosition = logRectTransform.anchoredPosition;
            offscreenAnchoredPosition = new Vector2(originalAnchoredPosition.x, originalAnchoredPosition.y + logRectTransform.rect.height + 100f);
            logRectTransform.anchoredPosition = offscreenAnchoredPosition;
        }
        private void OnReactionFinished(bool completed)
        {
            throw new System.NotImplementedException();
        }
        
        private IEnumerator AnimateTransition(bool show)
        {
            isLogVisible = show;
            
            float elapsed = 0f;
            Vector2 startPos = show ? offscreenAnchoredPosition : originalAnchoredPosition;
            Vector2 endPos = show ? originalAnchoredPosition : offscreenAnchoredPosition;

            // if (show)
            //     logImage.enabled = true;

            while (elapsed < animationDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / animationDuration);
                logRectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
                yield return null;
            }
            logRectTransform.anchoredPosition = endPos;

            // if (!show)
            //     logImage.enabled = false;
        }
    }
}