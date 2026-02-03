using Runtime.Scripts.Animation;
using Tree;
using UnityEngine;
using CharacterData = Core.CharacterData;

public class CameraMovementTrigger : MonoBehaviour
{
    [SerializeField] private CameraMovement camMovement;
    [SerializeField] private bool currentStatus;
    [SerializeField] private CharacterData commentCharacter;
    private void Awake()
    {
        camMovement = GetComponent<CameraMovement>();
        DialogTreeRunner.OnDialogRunningStatusChanged += OnDialogRunningStatusChanged;
    }

    private void OnDialogRunningStatusChanged(bool isRunning, DialogTree tree)
    {
        if (tree.Blackboard.CharacterData == commentCharacter)
            return;
        
        if(currentStatus != isRunning)
            camMovement.ToggleDialogMode(!isRunning);
        
        currentStatus = isRunning;
    }
}
