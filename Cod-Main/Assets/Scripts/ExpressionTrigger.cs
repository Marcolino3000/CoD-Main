#if UNITY_EDITOR
using System.Collections.Generic;
using Editor.AudioEditor;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace
{
    public class ExpressionTrigger : SerializedMonoBehaviour
    {
        public Dictionary<MarkerType, Sprite> Expressions;
        
        [SerializeField] private MarkerManager markerManager;

        private void OnEnable()
        {
            markerManager.OnMarkerReached += TriggerExpression;
        }

      #if UNITY_EDITOR
        private void TriggerExpression(MarkerType type)
        {
            if (type == MarkerType.Paragraph)
                return;
            
            // if (Expressions.TryGetValue(type, out var sprite))
            
        }
         #endif
    }

    // public class ExpressionMapper
    // {
    //     public MarkerType Expression;
    //     public Sprite Sprite;
    // }
}
#endif
