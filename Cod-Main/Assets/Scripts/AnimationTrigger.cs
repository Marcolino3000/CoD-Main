using System;
using Runtime.Scripts.PlayerInput;
using Unity.Mathematics;
// using Unity.Mathematics;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController _playerController;

    private void OnEnable()
    {
        _playerController.OnMovementStarted += TriggerAnimation;
        _playerController.OnMovementEnded += StopAnimation;
    }

    [ContextMenu("Trigger Animation")]
    private void TriggerAnimation(bool isMoving, MoveDirection moveDirection)
    {
        animator.SetBool("isWalking", true);
        
        FlipSprite(moveDirection);
    }

    private void FlipSprite(MoveDirection moveDirection)
    {
        transform.rotation = moveDirection switch
        {
            MoveDirection.Left => Quaternion.Euler(0, 0, 0),
            MoveDirection.Right => Quaternion.Euler(0, 180, 0),
            _ => transform.rotation
        };
    }

    [ContextMenu("stop animation")]
    private void StopAnimation()
    {
        animator.SetBool("isWalking", false);
    }
}
