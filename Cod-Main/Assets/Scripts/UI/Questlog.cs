using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Questlog : MonoBehaviour
    {
        [SerializeField] private float countdownUntilShow;
        [SerializeField] private float showDuration;
        [SerializeField] private Image logImage;
        
        private void Start()
        {
            StartCoroutine(ShowQuestLog());
        }

        private IEnumerator ShowQuestLog()
        {
            yield return StartCoroutine(StartCountdown(countdownUntilShow));
            SetQuestLogVisible(true);
            yield return StartCoroutine(StartCountdown(showDuration));
            SetQuestLogVisible(false);
        }

        private void SetQuestLogVisible(bool visible)
        {
            logImage.enabled = visible;
        }

        private IEnumerator StartCountdown(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
        }
    }
}