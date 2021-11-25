using UnityEngine;

public class ScriptImporter : MonoBehaviour, IDecoder
{
    [SerializeField] private TextAsset _narrativeScript;
    [SerializeField] private ObjectInventory _actorInventory;
    [SerializeField] private ObjectInventory _evidenceInventory;

    public IActorController ActorController { get; private set; }
    public ISceneController SceneController { get; } = new ScenePreloader();
    public IAudioController AudioController { get; } = new AudioPreloader();
    public IEvidenceController EvidenceController { get; private set; }
    public IAppearingDialogueController AppearingDialogueController { get; set; }

    private ScriptDecoder _scriptDecoder;

    private void Awake()
    {
        ActorController = new ObjectPreloader(_actorInventory);
        EvidenceController = new ObjectPreloader(_evidenceInventory);
        _scriptDecoder = new ScriptDecoder(this, _narrativeScript);
    }
}
