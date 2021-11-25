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
        LoadObject(actor);
    }

    public void SetActiveSpeaker(string actor)
    {
        LoadObject(actor);
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
        LoadObject(actor);
    }

    public void AddEvidence(string evidence)
    {
        LoadObject(evidence);
    }

    public void RemoveEvidence(string evidence)
    {
    }

    public void AddToCourtRecord(string actor)
    {
        LoadObject(actor);
    }

    public void RequirePresentEvidence()
    {
    }

    public void SubstituteEvidence(string originalEvidenceName, string newEvidenceName)
    {
        LoadObject(newEvidenceName);
    }

    public void OnPresentEvidence(ICourtRecordObject evidence)
    {
    }
    
    private void LoadObject(string objectName)
    {
        if (_inventory.ContainsKey(objectName)) return;
        
        _inventory.Add(Resources.Load<ActorData>(objectName));
        Debug.Log($"Loaded {objectName}");
    }
}
