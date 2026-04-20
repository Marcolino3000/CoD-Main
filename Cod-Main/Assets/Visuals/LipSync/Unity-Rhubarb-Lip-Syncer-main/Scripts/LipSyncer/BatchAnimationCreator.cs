using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BatchAnimationCreator : MonoBehaviour
{
    [SerializeField] private LipSyncer2D lipSyncer2D;
    [SerializeField] private string resourcesSubfolder;
    [SerializeField] private List<AudioClip> audioClips;
    
    [ContextMenu("Create Animations")]
    public void CreateAnimations()
    {
        audioClips = LoadAllClips();

        foreach (var clip in audioClips)
        {
            lipSyncer2D.sourceAudio = clip;
            lipSyncer2D.animationName = clip.name + "_Animation";
            
            lipSyncer2D.RhubarbAnalysis();  
        }
    }

    private List<AudioClip> LoadAllClips()
    {
        var clips = Resources.LoadAll<AudioClip>(resourcesSubfolder).ToList();
        if (clips == null || clips.Count == 0)
            Debug.LogWarning($"No AudioClips found in Resources/{resourcesSubfolder}");
        return clips;
    }
}