using System.Collections.Generic;
using UnityEngine;

public class ActorPreloader : IActorController
{
    private ObjectInventory _inventory;
    
    public ActorPreloader(ObjectInventory inventory)
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
    
    private void LoadActor(string actor)
    {
        if (_inventory.ContainsKey(actor)) return;
        
        _inventory.Add(Resources.Load<ActorData>(actor));
        Debug.Log($"Loaded {actor}");
    }
}
