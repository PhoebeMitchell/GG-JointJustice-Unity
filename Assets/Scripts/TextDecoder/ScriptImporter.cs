using Ink.Runtime;
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

    private ScriptReader _scriptReader;

    private void Awake()
    {
        ActorController = new ObjectPreloader(_actorInventory);
        EvidenceController = new ObjectPreloader(_evidenceInventory);
        _scriptReader = new ScriptReader(this, new Story(_narrativeScript.text));
    }
}
