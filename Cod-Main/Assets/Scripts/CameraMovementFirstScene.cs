using System.Collections;
using DefaultNamespace;
using UnityEngine;

public class CameraMovementFirstScene : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float duration;
    
    [Header("References")]
    [SerializeField] private Countdown countdown;
    
    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
        countdown = GetComponent<Countdown>();
        Countdown.OnCountdownFinished += MoveCameraToPosition;
    }

    private void MoveCameraToPosition()
    {
        StartCoroutine(MoveCameraCoroutine(targetPosition, duration));
    }

    private IEnumerator MoveCameraCoroutine(Vector3 vector3, float f)
    {
        while (cam.transform.position != vector3)
        {
            
        }
    }
}
