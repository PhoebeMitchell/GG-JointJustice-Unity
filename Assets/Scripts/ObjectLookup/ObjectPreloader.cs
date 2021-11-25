using UnityEngine;

public class ObjectPreloader : IActorController, IEvidenceController
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
        LoadObject(actorName, "Actors");
    }
    
    private void LoadEvidence(string evidenceName)
    {
        LoadObject(evidenceName, "Evidence");
    }
    
    private void LoadObject(string objectName, string directory)
    {
        if (_inventory.ContainsKey(objectName)) return;
        
        _inventory.Add(Resources.Load<ActorData>($"{directory}/{objectName}"));
    }
}
