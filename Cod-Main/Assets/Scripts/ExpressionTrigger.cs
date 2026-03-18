using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace
{
    public class ExpressionTrigger : SerializedMonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private MarkerManager markerManager;

        private void OnEnable()
        {
            markerManager.OnMarkerReached += TriggerExpression;
            AudioClipPlayer.FinishedPlaying += ResetExpressionToDefault;
        }

        private void ResetExpressionToDefault()
        {
            animator.SetInteger("expState", 1);
        }

        private void TriggerExpression(MarkerManager.MarkerType type)
        {
            // Debug.Log("triggered expression: " + type);
            
            if (type == MarkerManager.MarkerType.Paragraph)
                return;
            
            animator.SetInteger("expState", (int)type);
            
        }
    }
}
