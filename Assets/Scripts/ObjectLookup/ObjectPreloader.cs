using UnityEngine;

public class ObjectPreloader : IActorController, IEvidenceController, ISceneController, IAudioController
{
    private readonly ObjectInventory _inventory;
    
    public ObjectPreloader(ObjectInventory inventory)
    {
        _inventory = inventory;
    }

    public void SetActiveActor(string actor)
    {
        LoadActor(actor);
    }

    public void SetActiveSpeaker(string actor)
    {
        LoadActor(actor);
    }

    public void SetPose(string pose, string actorName = null)
    {
    }

    public void PlayEmotion(string emotion, string actorName = null)
    {
    }

    public void StartTalking()
    {
    }

    public void StopTalking()
    {
    }

    public void OnAnimationDone()
    {
    }

    public void SetSpeakingType(SpeakingType speakingType)
    {
    }
    
    public void AssignActorToSlot(string actor, int oneBasedSlotIndex)
    {
        LoadActor(actor);
    }

    public void AddEvidence(string evidence)
    {
        LoadEvidence(evidence);
    }

    public void RemoveEvidence(string evidence)
    {
    }

    public void AddToCourtRecord(string actor)
    {
        LoadActor(actor);
    }

    public void RequirePresentEvidence()
    {
    }

    public void SubstituteEvidence(string originalEvidenceName, string newEvidenceName)
    {
        LoadEvidence(newEvidenceName);
    }

    public void OnPresentEvidence(ICourtRecordObject evidence)
    {
    }

    private void LoadActor(string actorName)
    {
        if (_inventory.ContainsKey(actorName))
            return;
        
        _inventory.Add(Resources.Load<ActorData>($"Actors/{actorName}"));
    }
    
    private void LoadEvidence(string evidenceName)
    {
        if (_inventory.ContainsKey(evidenceName))
            return;
        
        _inventory.Add(Resources.Load<Evidence>($"Evidence/{evidenceName}"));
    }

    public void FadeIn(float seconds)
    {
    }

    public void FadeOut(float seconds)
    {
    }

    public void ShakeScreen(float intensity, float duration, bool isBlocking)
    {
    }

    public void SetScene(string background)
    {
    }

    public void SetCameraPos(Vector2Int position)
    {
    }

    public void PanCamera(float seconds, Vector2Int finalPosition)
    {
    }

    public void PanToActorSlot(int oneBasedSlotIndex, float seconds)
    {
    }

    public void JumpToActorSlot(int oneBasedSlotIndex)
    {
    }

    public void ShowItem(string item, ItemDisplayPosition position)
    {
    }

    public void ShowActor()
    {
    }

    public void HideActor()
    {
    }

    public void Wait(float seconds)
    {
    }

    public void HideItem()
    {
    }

    public void PlayAnimation(string animationName)
    {
    }

    public void PlaySFX(string SFX)
    {
    }

    public void PlaySong(string songName)
    {
    }

    public void StopSong()
    {
    }
}
