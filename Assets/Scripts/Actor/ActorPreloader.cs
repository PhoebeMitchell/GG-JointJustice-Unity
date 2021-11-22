using UnityEngine;

public class ActorPreloader : IActorController
{
    public void SetActiveActor(string actor)
    {
        Resources.Load(actor);
        Debug.Log($"Loaded {actor}");
    }

    public void SetActiveSpeaker(string actor)
    {
        Resources.Load(actor);
        Debug.Log($"Loaded {actor}");
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
        Resources.Load(actor);
        Debug.Log($"Loaded {actor}");
    }
}
