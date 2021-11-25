using System.Collections;
using UnityEngine;

public class ScriptImporter : MonoBehaviour, IDecoder
{
    [SerializeField] private TextAsset _narrativeScript;
    [SerializeField] private ActorInventory _actorInventory;
    [SerializeField] private EvidenceInventory _evidenceInventory;

    public IActorController ActorController { get; } = new ActorPreloader();
    public ISceneController SceneController { get; } = new ScenePreloader();
    public IAudioController AudioController { get; } = new AudioPreloader();
    public IEvidenceController EvidenceController { get; } = new EvidencePreloader();
    public IAppearingDialogueController AppearingDialogueController { get; set; }

    private ScriptDecoder _scriptDecoder;

    private void Awake()
    {
        _scriptDecoder = new ScriptDecoder(this, _narrativeScript);
    }

    public void ContinueStory()
    {
        _scriptDecoder.NextAction();
    }
}
