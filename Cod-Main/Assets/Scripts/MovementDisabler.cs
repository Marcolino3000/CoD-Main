using System;
using Runtime.Scripts.PlayerInput;
using Tree;
using UnityEngine;

namespace DefaultNamespace
{
    public class MovementDisabler : MonoBehaviour
    {
        private void OnEnable()
        {
            DialogTreeRunner.OnDialogRunningStatusChanged += HandleDialogStatusChanged;
        }

        private void HandleDialogStatusChanged(bool isRunning, DialogTree tree)
        {
            PlayerController.EnableMovement(!isRunning);
        }
    }
}