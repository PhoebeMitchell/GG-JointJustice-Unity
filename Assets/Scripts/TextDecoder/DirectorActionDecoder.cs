using UnityEngine;
using UnityEngine.Events;

public class DirectorActionDecoder : MonoBehaviour, IDecoder
{
    [Header("Events")]
    [Tooltip("Event that gets called when the system is done processing the action")]
    [SerializeField] private UnityEvent _onActionDone;

    private ActionDecoder _decoder;
    
    public IActorController ActorController { get; set; }
    public ISceneController SceneController { get; set; }
    public IAudioController AudioController { get; set; }
    public IEvidenceController EvidenceController { get; set; }
    public IAppearingDialogueController AppearingDialogueController { get; set; }

    private void Awake()
    {
        _decoder = new ActionDecoder(this);
        // We wrap this in an Action so we have no ties to UnityEngine in the ActionDecoder
        _decoder.OnActionDone += () => _onActionDone.Invoke();
    }

    #region API
    /// <summary>
    /// Called whenever a new action is executed (encountered and then forwarded here) in the script
    /// </summary>
    /// <param name="line">The full line in the script containing the action and parameters</param>
    public void OnNewActionLine(string line)
    {
        try
        {
            _decoder.OnNewActionLine(line);
        }
        catch (TextDecoder.Parser.ScriptParsingException exception)
        {
            Debug.LogError(exception);
            _onActionDone.Invoke();
        }
    }

    #endregion
}