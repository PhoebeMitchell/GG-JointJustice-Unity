public interface IDecoder
{
    IActorController ActorController { get; }
    ISceneController SceneController { get; }
    IAudioController AudioController { get; }
    IEvidenceController EvidenceController { get; }
    IAppearingDialogueController AppearingDialogueController { get; }
}
