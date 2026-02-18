using System.Collections;
using Unity.Cinemachine;
using Unity.Mathematics.Geometry;
using UnityEngine;

namespace DefaultNamespace
{
    public class TiltCameraMovement : MonoBehaviour
    {
        [SerializeField] private float _duration;
        [SerializeField] private AnimationCurve _ease;
        [SerializeField] private Vector3 _dialogFollowOffset;
        [SerializeField] private float _dialogTiltOffset;
        [SerializeField] private bool toggleOnDialog = true;
        [SerializeField] private CinemachinePanTilt panTilt;
        [SerializeField] private Transform marleneTransform;
        
        private Vector3 _followOffset;
        private float _tiltOffset;
        private CinemachineFollow _follow;
        private bool _isInDialogMode;
        private float _progress;

        private void Awake()
        {
            _follow = GetComponent<CinemachineFollow>();
            _followOffset = _follow.FollowOffset;
            _tiltOffset = panTilt.TiltAxis.Value;
        }
    
        public void ToggleDialogMode(bool isDialogRunning)
        {
            if (!toggleOnDialog)
                return;

            _isInDialogMode = isDialogRunning;
            
            StartCoroutine(DoProgress());
        }
        
        public void ToggleDialogMode(bool isDialogRunning, Vector3 secondCharacterPosition)
        {
            if (!toggleOnDialog)
                return;

            _isInDialogMode = isDialogRunning;

            if (_follow != null && _follow.FollowTarget != null)
            {
                Vector3 mainTargetPosition = marleneTransform.localPosition;
                float halfDistance = Vector3.Distance(mainTargetPosition, secondCharacterPosition) * 0.5f;
                // float midpointX = ((mainTargetPosition.x + secondCharacterPosition.x) * 0.5f) + mainTargetPosition.x;
                if(mainTargetPosition.x > secondCharacterPosition.x)
                    _dialogFollowOffset = new Vector3(-halfDistance, _dialogFollowOffset.y, _dialogFollowOffset.z);
                else
                    _dialogFollowOffset = new Vector3(halfDistance, _dialogFollowOffset.y, _dialogFollowOffset.z);
            }

            StartCoroutine(DoProgress());
        }

        private IEnumerator DoProgress()
        {
            while (_progress < 1.0f)
            {
                _progress += Time.deltaTime / _duration;
            
                // onValueChanged?.Invoke(_progress);
                TweenUpdate(_ease.Evaluate(_progress));
                yield return _progress;
            }
        
            _progress = 0.0f;
            _isInDialogMode = !_isInDialogMode;
        }

        private void TweenUpdate(float progress)
        {
            _follow.FollowOffset = _isInDialogMode ? 
                Vector3.Lerp(_dialogFollowOffset, _followOffset, progress) : 
                Vector3.Lerp(_followOffset, _dialogFollowOffset, progress);
         
            panTilt.TiltAxis.Value = _isInDialogMode ? 
                Mathf.Lerp(_dialogTiltOffset, _tiltOffset, progress) : 
                Mathf.Lerp(_tiltOffset, _dialogTiltOffset, progress);
                
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(500, 10, 140, 30), "Toggle Cam DialogMode"))
            {
                _isInDialogMode = !_isInDialogMode;
                ToggleDialogMode(_isInDialogMode);
            }
        }
    }
}