using System;
using UnityEngine;

public class UnknownCommandException : Exception
{
    public string CommandName { get; private set; }

    public UnknownCommandException(string commandName)
    {
        CommandName = commandName;
    }
}

public class InvalidSyntaxException : Exception
{
    public InvalidSyntaxException()
    {
    }
}

public enum ActionName
{
    ACTOR,
    SET_ACTOR_POSITION,
    SHOWACTOR,
    SPEAK,
    THINK,
    SET_POSE,
    PLAY_EMOTION,
    PLAYSFX,
    PLAYSONG,
    STOP_SONG,
    FADE_OUT,
    FADE_IN,
    CAMERA_PAN,
    CAMERA_SET,
    SHAKESCREEN,
    SCENE,
    WAIT,
    SHOW_ITEM,
    HIDE_ITEM,
    PLAY_ANIMATION,
    JUMP_TO_POSITION,
    PAN_TO_POSITION,
    ADD_EVIDENCE,
    REMOVE_EVIDENCE,
    ADD_RECORD,
    PRESENT_EVIDENCE,
    SUBSTITUTE_EVIDENCE,
    DIALOG_SPEED,
    OVERALL_SPEED,
    PUNCTUATION_SPEED,
    CLEAR_SPEED,
    DISABLE_SKIPPING,
    AUTOSKIP,
    CONTINUE_DIALOG,
    APPEAR_INSTANTLY,
    HIDE_TEXTBOX,
    WAIT_FOR_INPUT
}

public class ActionDecoder
{
    /// <summary>
    /// Forwarded from the DirectorActionDecoder
    /// </summary>
    public event Action OnActionDone;

    public IActorController ActorController { get; set; }
    public ISceneController SceneController { get; set; }
    public IAudioController AudioController { get; set; }
    public IEvidenceController EvidenceController { get; set; }
    public IAppearingDialogueController AppearingDialogueController { get; set; } = null;

    public ActionDecoder()
    {
    }

    public void OnNewActionLine(ActionLine actionLine)
    {
        switch (actionLine.Action)
        {
            //Actor controller
            case ActionName.ACTOR: SetActor(actionLine.NextString("actor name")); break;
            case ActionName.SET_ACTOR_POSITION: SetActorPosition(actionLine.NextOneBasedInt("slot index"), actionLine.NextString("actor name")); break;
            case ActionName.SHOWACTOR: SetActorVisibility(actionLine.NextBool("should show")); break;
            case ActionName.SPEAK: SetSpeaker(actionLine.NextString("actor name"), SpeakingType.Speaking); break;
            case ActionName.THINK: SetSpeaker(actionLine.NextString("actor name"), SpeakingType.Thinking); break;
            case ActionName.SET_POSE: SetPose(actionLine.NextString("pose name"), actionLine.NextOptionalString("target actor")); break;
            case ActionName.PLAY_EMOTION: PlayEmotion(actionLine.NextString("emotion name"), actionLine.NextOptionalString("target actor")); break; //Emotion = animation on an actor. Saves PLAY_ANIMATION for other things
            //Audio controller
            case ActionName.PLAYSFX: PlaySFX(actionLine.NextString("sfx name")); break;
            case ActionName.PLAYSONG: SetBGMusic(actionLine.NextString("song name")); break;
            case ActionName.STOP_SONG: StopSong(); break;
            //Scene controller
            case ActionName.FADE_OUT: FadeOutScene(actionLine.NextFloat("seconds")); break;
            case ActionName.FADE_IN: FadeInScene(actionLine.NextFloat("seconds")); break;
            case ActionName.CAMERA_PAN: PanCamera(actionLine.NextFloat("duration"), actionLine.NextInt("x"), actionLine.NextInt("y")); break;
            case ActionName.CAMERA_SET: SetCameraPosition(actionLine.NextInt("x"), actionLine.NextInt("y")); break;
            case ActionName.SHAKESCREEN: ShakeScreen(actionLine.NextFloat("intensity")); break;
            case ActionName.SCENE: SetScene(actionLine.NextString("scene name")); break;
            case ActionName.WAIT: Wait(actionLine.NextFloat("seconds")); break;
            case ActionName.SHOW_ITEM: ShowItem(actionLine.NextString("item name"), actionLine.NextEnumValue<ItemDisplayPosition>("item position")); break;
            case ActionName.HIDE_ITEM: HideItem(); break;
            case ActionName.PLAY_ANIMATION: PlayAnimation(actionLine.NextString("animation name")); break;
            case ActionName.JUMP_TO_POSITION: JumpToActorSlot(actionLine.NextOneBasedInt("slot index")); break;
            case ActionName.PAN_TO_POSITION: PanToActorSlot(actionLine.NextOneBasedInt("slot index"), actionLine.NextFloat("pan duration")); break;
            //Evidence controller
            case ActionName.ADD_EVIDENCE: AddEvidence(actionLine.NextString("evidence name")); break;
            case ActionName.REMOVE_EVIDENCE: RemoveEvidence(actionLine.NextString("evidence name")); break;
            case ActionName.ADD_RECORD: AddToCourtRecord(actionLine.NextString("evidence name")); break;
            case ActionName.PRESENT_EVIDENCE: OpenEvidenceMenu(); break;
            case ActionName.SUBSTITUTE_EVIDENCE: SubstituteEvidence(actionLine.NextString("evidence name")); break;
            //Dialog controller
            case ActionName.DIALOG_SPEED: ChangeDialogSpeed(WaiterType.Dialog, actionLine.NextFloat("seconds")); break;
            case ActionName.OVERALL_SPEED: ChangeDialogSpeed(WaiterType.Overall, actionLine.NextFloat("seconds")); break;
            case ActionName.PUNCTUATION_SPEED: ChangeDialogSpeed(WaiterType.Punctuation, actionLine.NextFloat("seconds")); break;
            case ActionName.CLEAR_SPEED: ClearDialogSpeeds(); break;
            case ActionName.DISABLE_SKIPPING: DisableTextSkipping(actionLine.NextBool("is disabled")); break;
            case ActionName.AUTOSKIP: AutoSkip(actionLine.NextBool("is on")); break;
            case ActionName.CONTINUE_DIALOG: ContinueDialog(); break;
            case ActionName.APPEAR_INSTANTLY: AppearInstantly(); break;
            case ActionName.HIDE_TEXTBOX: HideTextbox(); break;
            //Do nothing
            case ActionName.WAIT_FOR_INPUT: break;
            //Default
            // If we got here then the action exists in the ActionName enum but doesn't have a case in the switch hooked up to it.
            default: throw new UnknownCommandException(actionLine.Action.ToString());
        }
    }

    #region DialogStuff
    /// <summary>
    /// Checks if the decoder has an appearing dialog controller attached, and shows an error if it doesn't
    /// </summary>
    /// <returns>Whether an appearing dialog controller is connected</returns>
    private bool HasAppearingDialogController()
    {
        if (AppearingDialogueController == null)
        {
            Debug.LogError("No appearing dialog controller attached to the action decoder");
            return false;
        }
        return true;
    }

    ///<summary>
    ///Changes the dialog speed in appearingDialogController if it has beben set.
    ///</summary>
    ///<param name = "currentWaiterType">The current waiters type which appear time should be changed.</param>
    ///<param name = "parameters">Contains all the parameters needed to change the appearing time.</param>
    private void ChangeDialogSpeed(WaiterType currentWaiterType, float seconds)
    {
        if (!HasAppearingDialogController())
            return;

        AppearingDialogueController.SetTimerValue(currentWaiterType, seconds);
    }

    ///<summary>
    ///Clears all custom set dialog speeds
    ///</summary>
    private void ClearDialogSpeeds()
    {
        if (!HasAppearingDialogController())
            return;

        AppearingDialogueController.ClearAllWaiters();
    }

    ///<summary>
    ///Toggles skipping on or off
    ///</summary>
    ///<param name = "disable">Should the text skipping be disabled or not</param>
    private void DisableTextSkipping(bool value)
    {
        if (!HasAppearingDialogController())
            return;

        AppearingDialogueController.ToggleDisableTextSkipping(value);
    }

    ///<summary>
    ///Makes the new dialog appear after current one.
    ///</summary>
    private void ContinueDialog()
    {
        if (!HasAppearingDialogController())
            return;

        AppearingDialogueController.ContinueDialog();
    }

    ///<summary>
    ///Forces the next line of dialog happen right after current one.
    ///</summary>
    private void AutoSkip(bool value)
    {
        if (!HasAppearingDialogController())
            return;

        AppearingDialogueController.AutoSkipDialog(value);
    }

    /// <summary>
    /// Makes the next line of dialogue appear instantly instead of one character at a time.
    /// </summary>
    private void AppearInstantly()
    {
        if (!HasAppearingDialogController())
            return;

        AppearingDialogueController.PrintTextInstantly = true;
        OnActionDone?.Invoke();
    }

    /// <summary>
    /// Hides the dialogue textbox.
    /// </summary>
    private void HideTextbox()
    {
        if (!HasAppearingDialogController())
            return;

        AppearingDialogueController.HideTextbox();
        OnActionDone?.Invoke();
    }
    #endregion

    #region EvidenceController
    /// <summary>
    /// Checks if the decoder has an evidence controller attached, and shows an error if it doesn't
    /// </summary>
    /// <returns>Whether an evidence controller is connected</returns>
    private bool HasEvidenceController()
    {
        if (EvidenceController == null)
        {
            Debug.LogError("No evidence controller attached to the action decoder");
            return false;
        }
        return true;
    }

    private void AddEvidence(string evidence)
    {
        if (!HasEvidenceController())
            return;

        EvidenceController.AddEvidence(evidence);
        OnActionDone?.Invoke();
    }

    private void RemoveEvidence(string evidence)
    {
        if (!HasEvidenceController())
            return;

        EvidenceController.RemoveEvidence(evidence);
        OnActionDone?.Invoke();
    }

    private void AddToCourtRecord(string actor)
    {
        if (!HasEvidenceController())
            return;

        EvidenceController.AddToCourtRecord(actor);
        OnActionDone?.Invoke();
    }

    /// <summary>
    /// Calls the onPresentEvidence event on evidence controller which
    /// opens the evidence menu so evidence can be presented.
    /// </summary>
    private void OpenEvidenceMenu()
    {
        if (!HasEvidenceController())
            return;

        EvidenceController.OpenEvidenceMenu();
    }

    /// <summary>
    /// Used to substitute a specified Evidence object with its assigned alternate Evidence object.
    /// </summary>
    /// <param name="evidence">The name of the evidence to substitute.</param>
    private void SubstituteEvidence(string evidence)
    {
        if (!HasEvidenceController())
            return;

        EvidenceController.SubstituteEvidenceWithAlt(evidence);
        OnActionDone?.Invoke();
    }

    #endregion


    #region AudioController
    /// <summary>
    /// Checks if the decoder has an audio controller attached, and shows an error if it doesn't
    /// </summary>
    /// <returns>Whether an audio controller is connected</returns>
    private bool HasAudioController()
    {
        if (AudioController == null)
        {
            Debug.LogError("No audio controller attached to the action decoder");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Plays a sound effect
    /// </summary>
    /// <param name="sfx">Name of the sound effect</param>
    private void PlaySFX(string sfx)
    {
        if (!HasAudioController())
            return;

        AudioController.PlaySFX(sfx);
        OnActionDone?.Invoke();
    }

    /// <summary>
    /// Sets the background music
    /// </summary>
    /// <param name="songName">Name of the new song</param>
    private void SetBGMusic(string songName)
    {
        if (!HasAudioController())
            return;

        AudioController.PlaySong(songName);
        OnActionDone?.Invoke();
    }

    /// <summary>
    /// If music is currently playing, stop it!
    /// </summary>
    private void StopSong()
    {
        if (!HasAudioController())
            return;

        AudioController.StopSong();
        OnActionDone?.Invoke();
    }
    #endregion

    #region SceneController
    /// <summary>
    /// Checks if the decoder has a scene controller attached, and shows an error if it doesn't
    /// </summary>
    /// <returns>Whether a scene controller is connected</returns>
    private bool HasSceneController()
    {
        if (SceneController == null)
        {
            Debug.LogError("No scene controller attached to the action decoder");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Fades the scene in from black
    /// </summary>
    /// <param name="seconds">Amount of seconds the fade-in should take as a float</param>
    private void FadeInScene(float timeInSeconds)
    {
        if (!HasSceneController())
            return;

        SceneController.FadeIn(timeInSeconds);
    }

    /// <summary>
    /// Fades the scene to black
    /// </summary>
    /// <param name="seconds">Amount of seconds the fade-out should take as a float</param>
    private void FadeOutScene(float timeInSeconds)
    {
        if (!HasSceneController())
            return;

        SceneController.FadeOut(timeInSeconds);

    }

    /// <summary>
    /// Shakes the screen
    /// </summary>
    /// <param name="intensity">Max displacement of the screen as a float</param>
    private void ShakeScreen(float intensity)
    {
        if (!HasSceneController())
            return;

        SceneController.ShakeScreen(intensity);
    }

    /// <summary>
    /// Sets the scene (background, character location on screen, any props (probably prefabs))
    /// </summary>
    /// <param name="sceneName">Scene to change to</param>
    private void SetScene(string sceneName)
    {
        if (!HasSceneController())
            return;

        SceneController.SetScene(sceneName);
        OnActionDone?.Invoke();
    }

    /// <summary>
    /// Sets the camera position
    /// </summary>
    /// <param name="parameters">New camera coordinates in the "int x,int y" format</param>
    private void SetCameraPosition(int x, int y)
    {
        if (!HasSceneController())
            return;

        SceneController.SetCameraPos(new Vector2Int(x, y));
        OnActionDone?.Invoke();
    }

    /// <summary>
    /// Pan the camera to a certain x,y position
    /// </summary>
    private void PanCamera(float duration, int x, int y)
    {
        if (!HasSceneController())
            return;

        SceneController.PanCamera(duration, new Vector2Int(x, y));
        OnActionDone?.Invoke();
    }

    /// <summary>
    /// Shows an item on the middle, left, or right side of the screen.
    /// </summary>
    /// <param name="parameters">Which item to show and where to show it, in the "string item, itemPosition pos" format</param>
    private void ShowItem(string item, ItemDisplayPosition itemPos)
    {
        if (!HasSceneController())
            return;

        SceneController.ShowItem(item, itemPos);
        OnActionDone?.Invoke();
    }

    /// <summary>
    /// Hides the item displayed on the screen by ShowItem method.
    /// </summary>
    private void HideItem()
    {
        if (!HasSceneController())
            return;

        SceneController.HideItem();
        OnActionDone?.Invoke();
    }

    /// <summary>
    /// Waits seconds before automatically continuing.
    /// </summary>
    /// <param name="seconds">Amount of seconds to wait</param>
    private void Wait(float seconds)
    {
        if (!HasSceneController())
            return;

        SceneController.Wait(seconds);
    }

    /// <summary>
    /// Plays a full screen animation e.g. Ross' galaxy brain or the gavel hit animations.
    /// </summary>
    /// <param name="animationName">The name of the animation to play.</param>
    private void PlayAnimation(string animationName)
    {
        if (!HasSceneController())
            return;

        SceneController.PlayAnimation(animationName);
    }

    /// Jump-cuts the camera to the target sub position if the bg-scene has sub positions.
    /// </summary>
    /// <param name="oneBasedSlotIndexAsString">String containing an integer referring to the target sub position, 1 based.</param>
    private void JumpToActorSlot(int slotIndex)
    {
        if (!HasSceneController())
        {
            return;
        }

        SceneController.JumpToActorSlot(slotIndex);
        OnActionDone?.Invoke();
    }

    /// <summary>
    /// Pans the camera to the target actor slot if the bg-scene has support for actor slots.
    /// </summary>
    /// <param name="parameters">String containing a one-based integer index referring to the target actor slot, and a floating point number referring to the amount of time the pan should take in seconds.</param>
    private void PanToActorSlot(int slotIndex, float panDuration)
    {
        if (!HasSceneController())
        {
            return;
        }

        SceneController.PanToActorSlot(slotIndex, panDuration);
        OnActionDone?.Invoke();
    }

    #endregion


    #region ActorController
    /// <summary>
    /// Checks if the decoder has an actor controller attached, and shows an error if it doesn't
    /// </summary>
    /// <returns>Whether an actor controller is connected</returns>
    private bool HasActorController()
    {
        if (ActorController == null)
        {
            Debug.LogError("No actor controller attached to the action decoder");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Sets the shown actor in the scene
    /// </summary>
    /// <param name="actor">Actor to be switched to</param>
    private void SetActor(string actor)
    {
        if (!HasActorController())
            return;

        ActorController.SetActiveActor(actor);
        OnActionDone?.Invoke();
    }

    /// <summary>
    /// Shows or hides the actor based on the string parameter.
    /// </summary>
    /// <param name="showActor">Should contain true or false based on showing or hiding the actor respectively</param>
    private void SetActorVisibility(bool shouldShow)
    {
        if (!HasSceneController())
            return;

        if (shouldShow)
        {
            SceneController.ShowActor();
        }
        else
        {
            SceneController.HideActor();
        }

        OnActionDone?.Invoke();
    }

    /// <summary>
    /// Set the speaker for the current and following lines, until a new speaker is set
    /// </summary>
    /// <param name="actor">Actor to make the speaker</param>
    /// <param name="speakingType">Type of speaking to speak the text with</param>
    private void SetSpeaker(string actor, SpeakingType speakingType)
    {
        if (!HasActorController())
            return;

        ActorController.SetActiveSpeaker(actor);
        ActorController.SetSpeakingType(speakingType);
        OnActionDone?.Invoke();
    }

    /// <summary>
    /// Set the pose of the current actor
    /// </summary>
    /// <param name="parameters">"[pose name]" to set pose for current actor OR "[pose name],[actor name]" to set pose for another actor</param>
    private void SetPose(string poseName, string optional_targetActor)
    {
        if (!HasActorController())
            return;

        if (optional_targetActor == null)
        {
            ActorController.SetPose(poseName);
            OnActionDone?.Invoke();
        }
        else
        {
            ActorController.SetPose(poseName, optional_targetActor);
            OnActionDone?.Invoke();
        }
    }

    /// <summary>
    /// Plays an emotion for the current actor. Emotion is a fancy term for animation on an actor.
    /// </summary>
    /// <param name="parameters">"[animation name]" to set pose for current actor OR "[animation name],[actor name]" to queue animation for another actor (gets played as soon as actor is visible)</param>
    private void PlayEmotion(string poseName, string optional_targetActor)
    {
        if (!HasActorController())
            return;

        if (optional_targetActor == null)
        {
            ActorController.PlayEmotion(poseName);
        }
        else
        {
            ActorController.PlayEmotion(poseName, optional_targetActor);
        }
    }

    /// <summary>
    /// Sets an actor to a specific slot in the currently active scene.
    /// </summary>
    /// <param name="parameters">String containing the actor name first and one-based slot index second.</param>
    private void SetActorPosition(int oneBasedSlotIndex, string actorName)
    {
        if (!HasActorController())
        {
            return;
        }

        ActorController.AssignActorToSlot(actorName, oneBasedSlotIndex);
        OnActionDone?.Invoke();
    }
    #endregion

}
