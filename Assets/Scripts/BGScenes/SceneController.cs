using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class SceneController : MonoBehaviour, ISceneController
{
    [Tooltip("List of BG scenes in the unity scene, needs to be dragged here for every scene")]
    [SerializeField] private BGSceneList _sceneList;

    [FormerlySerializedAs("_fadeToImageController")]
    [Tooltip("Drag a FadeToImageController here.")]
    [SerializeField] private FadeToImageTransition _fadeToImageTransition;

    [Header("Events")]
    [Tooltip("Attach the action decoder object here")]
    [SerializeField] DirectorActionDecoder _directorActionDecoder;

    [Tooltip("This event is called when a wait action is started.")]
    [SerializeField] private UnityEvent _onWaitStart;

    [Tooltip("This event is called when a wait action is finished.")]
    [SerializeField] private UnityEvent _onWaitComplete;

    [Tooltip("Event that gets called when the actor displayed on screen changes")]
    [SerializeField] private UnityEvent<Actor> _onActorChanged;



    private Coroutine _waitCoroutine;
    
    /// <summary>
    /// Called when the object is initialized
    /// </summary>
    void Start()
    {
        if (_directorActionDecoder == null)
        {
            Debug.LogError("Scene Controller doesn't have a action decoder to attach to");
        }
        else
        {
            _directorActionDecoder.SetSceneController(this);
        }

    }

    /// <summary>
    /// Fades an image from opaque to transparent. Used to fade in to a scene from a black screen.
    /// </summary>
    /// <param name="seconds">The number of seconds it should take to fade.</param>
    public void FadeIn(float seconds)
    {
        _onWaitStart.Invoke();
        StartCoroutine(_fadeToImageTransition.FadeImage(1, 0, seconds, _onWaitComplete));
    }

    /// <summary>
    /// Fades an image from transparent to opaque. Used to fade out from a scene to a black screen.
    /// </summary>
    /// <param name="seconds">The number of seconds it should take to fade.</param>
    public void FadeOut(float seconds)
    {
        _onWaitStart.Invoke();
        StartCoroutine(_fadeToImageTransition.FadeImage(0, 1, seconds, _onWaitComplete));
    }

    public void PanCamera(float seconds, Vector2Int position)
    {
        Debug.LogWarning("PanCamera not implemented");
    }

    /// <summary>
    /// Sets the new bg-scene based on the provided name and changes the active actor using an event.
    /// </summary>
    /// <param name="background">Target bg-scene</param>
    public void SetScene(string background)
    {
        BGScene newScene = _sceneList.SetScene(background);

        if (newScene != null)
        {
            _onActorChanged.Invoke(newScene.ActiveActor);
        }
    }

    public void SetCameraPos(Vector2Int position)
    {
        Debug.LogWarning("SetCameraPos not implemented");
    }

    public void ShakeScreen(float intensity)
    {
        Debug.LogWarning("ShakeScreen not implemented");
    }

    public void ShowItem(string item, itemDisplayPosition position)
    {
        Debug.LogWarning("ShowItem not implemented");
    }
    
    public void ShowActor()
    {
        Debug.LogWarning("ShowActor not implemented");
    }
    
    public void HideActor()
    {
        Debug.LogWarning("HideActor not implemented");
    }

    /// <summary>
    /// Starts and caches a coroutine to wait for a specific amount of time.
    /// Called by DirectorActionController when a WAIT action is read.
    /// </summary>
    /// <param name="seconds"></param>
    public void Wait(float seconds)
    {
        _waitCoroutine = StartCoroutine(WaitCoroutine(seconds));
    }

    /// <summary>
    /// Coroutine to handle waiting for a specific amount of time.
    /// </summary>
    /// <param name="seconds">The time to wait in seconds.</param>
    private IEnumerator WaitCoroutine(float seconds)
    {
        _onWaitStart.Invoke();
        yield return new WaitForSeconds(seconds);
        _onWaitComplete?.Invoke();
    }

    /// <summary>
    /// Cancels a wait coroutine. Used to stop the the coroutine from
    /// continuing if more actions are read before waiting is complete.
    /// Subscribe this to DialogueController's onNewActionLine and onNewSpokenLine events.
    /// Make sure the event is ABOVE DirectorActionController in the subscribed events list.
    /// </summary>
    public void CancelWaitCoroutine()
    {
        if (_waitCoroutine == null)
            return;
        
        StopCoroutine(_waitCoroutine);
        _waitCoroutine = null;
    }
}
