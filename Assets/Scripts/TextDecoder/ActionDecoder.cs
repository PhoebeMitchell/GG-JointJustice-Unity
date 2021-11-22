using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActionDecoder
{
    public event Action OnActionDone;

    private IDecoder _decoder;

    public ActionDecoder(IDecoder decoder)
    {
        _decoder = decoder;
    }
    
    /// <summary>
    /// Parse action lines inside .ink-files
    /// </summary>
    /// <param name="lines">Action line</param>
    /// <remarks>
    ///     Writers are able to call methods inside .ink files. This is done by using the following syntax:
    ///     <code>
    ///     &amp;{methodName}:{parameter1},{parameter2},...
    ///     </code>
    ///     This method is responsible for:
    ///         1. Finding a method inside this class, matching `methodName`
    ///         2. Verifying the amount of parameters matches the amount of parameters needed in the method
    ///         3. Attempting to parse each parameter into the correct type using <see cref="Parser<T>"/> of the type
    ///         4. Invoking the method with the parsed parameters
    /// </remarks>
    public void OnNewActionLine(string actionLine)
    {
        ActionNode method = Decode(actionLine);

        // Call the method
        // method.Info.Invoke(this, method.Parameters);
        method.Execute();
    }

    public ActionNode Decode(string actionLine)
    {
        actionLine = actionLine.Trim();
        const char actionSideSeparator = ':';
        const char actionParameterSeparator = ',';

        string[] actionNameAndParameters = actionLine.Substring(1, actionLine.Length - 1).Trim().Split(actionSideSeparator);

        if (actionNameAndParameters.Length > 2)
        {
            throw new TextDecoder.Parser.ScriptParsingException($"More than one '{actionSideSeparator}' detected in line '{actionLine}'");
        }

        string action = actionNameAndParameters[0];
        string[] parameters = (actionNameAndParameters.Length == 2) ? actionNameAndParameters[1].Split(actionParameterSeparator) : Array.Empty<string>();

        // Find method with exact same name as action inside script
        MethodInfo method = GetType().GetMethod(action, BindingFlags.Instance | BindingFlags.NonPublic);
        if (method == null)
        {
            throw new TextDecoder.Parser.ScriptParsingException($"DirectorActionDecoder contains no method named '{action}'");
        }

        ParameterInfo[] methodParameters = method.GetParameters();
        var optionalParameters = methodParameters.Count(parameter => parameter.IsOptional);
        if (parameters.Length < (methodParameters.Length - optionalParameters) || parameters.Length > (methodParameters.Length))
        {
            throw new TextDecoder.Parser.ScriptParsingException($"'{action}' requires {(optionalParameters == 0 ? "exactly" : "between")} {(optionalParameters == 0 ? methodParameters.Length.ToString() : $"{methodParameters.Length-optionalParameters} and {methodParameters.Length}")} parameters (has {parameters.Length} instead)");
        }

        List<object> parsedMethodParameters = new List<object>();
        // For each supplied parameter of that action...
        for (int index = 0; index < parameters.Length; index++)
        {
            if (parameters.Length <= index && methodParameters[index].IsOptional)
            {
                parsedMethodParameters.Add(methodParameters[index].DefaultValue);
            }

            // Determine it's type
            ParameterInfo methodParameter = methodParameters[index];

            // Construct a parser for it
            Type parser = GetType().Assembly.GetTypes().FirstOrDefault(type => type.BaseType is { IsGenericType: true } && type.BaseType.GenericTypeArguments[0] == methodParameter.ParameterType);
            if (parser == null)
            {
                throw new ArgumentException($"The TextDecoder.Parser namespace contains no Parser for type {methodParameter.ParameterType}");
            }

            ConstructorInfo parserConstructor = parser.GetConstructor(Type.EmptyTypes);
            if (parserConstructor == null)
            {
                throw new ArgumentException($"TextDecoder.Parser for type {methodParameter.ParameterType} has no constructor without parameters");
            }

            // Find the 'Parse' method on that parser
            MethodInfo parseMethod = parser.GetMethod("Parse");
            if (parseMethod == null)
            {
                throw new ArgumentException($"TextDecoder.Parser for type {methodParameter.ParameterType} has no 'Parse' method");
            }

            // Create a parser and call the 'Parse' method
            object parserInstance = parserConstructor.Invoke(Array.Empty<object>());
            object[] parseMethodParameters = { parameters[index], null };

            // If we received an error attempting to parse a parameter to the type, expose it to the user
            var humanReadableParseError = parseMethod.Invoke(parserInstance, parseMethodParameters);
            if (humanReadableParseError != null)
            {
                throw new TextDecoder.Parser.ScriptParsingException($"'{parameters[index]}' is incorrect as parameter #{index + 1} ({methodParameter.Name}) for action '{action}': {humanReadableParseError}");
            }

            parsedMethodParameters.Add(parseMethodParameters[1]);
        }

        // If the method supports optional parameters, fill the remaining parameters based on the default value of the method
        for (int suppliedParameterCount = parameters.Length; suppliedParameterCount < methodParameters.Length; suppliedParameterCount++)
        {
            parsedMethodParameters.Add(methodParameters[suppliedParameterCount].DefaultValue);
        }

        return new ActionNode(method, parsedMethodParameters.ToArray(), _decoder);
    }

    // ReSharper disable InconsistentNaming
    // ReSharper disable UnusedMember.Local
#pragma warning disable IDE0051 // Remove unused private members
    #region DialogStuff
    private void DIALOG_SPEED(float seconds)
    {
        _decoder.AppearingDialogueController.SetTimerValue(WaiterType.Dialog, seconds);
    }
    private void OVERALL_SPEED(float seconds)
    {
        _decoder.AppearingDialogueController.SetTimerValue(WaiterType.Overall, seconds);
    }
    private void PUNCTUATION_SPEED(float seconds)
    {
        _decoder.AppearingDialogueController.SetTimerValue(WaiterType.Punctuation, seconds);
    }

    private void CLEAR_SPEED()
    {
        _decoder.AppearingDialogueController.ClearAllWaiters();
    }

    private void DISABLE_SKIPPING(bool value)
    {
        _decoder.AppearingDialogueController.ToggleDisableTextSkipping(value);
    }

    private void CONTINUE_DIALOG()
    {
        _decoder.AppearingDialogueController.ContinueDialog();
    }

    private void AUTO_SKIP(bool value)
    {
        _decoder.AppearingDialogueController.AutoSkipDialog(value);
    }

    private void APPEAR_INSTANTLY()
    {
        _decoder.AppearingDialogueController.PrintTextInstantly = true;
        OnActionDone?.Invoke();
    }

    private void HIDE_TEXTBOX()
    {
        _decoder.AppearingDialogueController.HideTextbox();
        OnActionDone?.Invoke();
    }
    #endregion

    #region EvidenceController
    private void ADD_EVIDENCE(string evidence)
    {
        _decoder.EvidenceController.AddEvidence(evidence);
        OnActionDone?.Invoke();
    }

    private void REMOVE_EVIDENCE(string evidence)
    {
        _decoder.EvidenceController.RemoveEvidence(evidence);
        OnActionDone?.Invoke();
    }

    private void ADD_RECORD(string actor)
    {
        _decoder.EvidenceController.AddToCourtRecord(actor);
        OnActionDone?.Invoke();
    }
    
    private void PRESENT_EVIDENCE()
    {
        _decoder.EvidenceController.RequirePresentEvidence();
    }

    private void SUBSTITUTE_EVIDENCE(string evidence)
    {
        _decoder.EvidenceController.SubstituteEvidenceWithAlt(evidence);
        OnActionDone?.Invoke();
    }

    #endregion


    #region AudioController
    private void PLAY_SFX(string sfx)
    {
        _decoder.AudioController.PlaySFX(sfx);
        OnActionDone?.Invoke();
    }

    private void PLAY_SONG(string songName)
    {
        _decoder.AudioController.PlaySong(songName);
        OnActionDone?.Invoke();
    }

    private void STOP_SONG()
    {
        _decoder.AudioController.StopSong();
        OnActionDone?.Invoke();
    }
    #endregion

    #region SceneController
    private void FADE_IN(float timeInSeconds)
    {
        _decoder.SceneController.FadeIn(timeInSeconds);
    }

    private void FADE_OUT(float timeInSeconds)
    {
        _decoder.SceneController.FadeOut(timeInSeconds);
    }

    private void SHAKE_SCREEN(float intensity, float duration, bool isBlocking)
    {
        _decoder.SceneController.ShakeScreen(intensity, duration, isBlocking);
    }

    private void SCENE(string sceneName)
    {
        _decoder.SceneController.SetScene(sceneName);
        OnActionDone?.Invoke();
    }

    private void CAMERA_SET(int x, int y)
    {
        _decoder.SceneController.SetCameraPos(new Vector2Int(x, y));
        OnActionDone?.Invoke();
    }

    private void CAMERA_PAN(float duration, int x, int y)
    {
        _decoder.SceneController.PanCamera(duration, new Vector2Int(x, y));
        OnActionDone?.Invoke();
    }

    private void SHOW_ITEM(string item, ItemDisplayPosition itemPos)
    {
        _decoder.SceneController.ShowItem(item, itemPos);
        OnActionDone?.Invoke();
    }

    private void HIDE_ITEM()
    {
        _decoder.SceneController.HideItem();
        OnActionDone?.Invoke();
    }

    private void WAIT(float seconds)
    {
        _decoder.SceneController.Wait(seconds);
    }

    private void PLAY_ANIMATION(string animationName)
    {
        _decoder.SceneController.PlayAnimation(animationName);
    }

    private void JUMP_TO_POSITION(int slotIndex)
    {
        _decoder.SceneController.JumpToActorSlot(slotIndex);
        OnActionDone?.Invoke();
    }

    private void PAN_TO_POSITION(int slotIndex, float panDuration)
    {
        _decoder.SceneController.PanToActorSlot(slotIndex, panDuration);
        OnActionDone?.Invoke();
    }

    #endregion


    #region ActorController
    private void ACTOR(string actor)
    {
        _decoder.ActorController.SetActiveActor(actor);
        OnActionDone?.Invoke();
    }

    private void SHOW_ACTOR(bool shouldShow)
    {
        if (shouldShow)
        {
            _decoder.SceneController.ShowActor();
        }
        else
        {
            _decoder.SceneController.HideActor();
        }

        OnActionDone?.Invoke();
    }

    private void SPEAK(string actor)
    {
        SetSpeaker(actor, SpeakingType.Speaking);
    }

    private void THINK(string actor)
    {
        SetSpeaker(actor, SpeakingType.Thinking);
    }

    private void SetSpeaker(string actor, SpeakingType speakingType)
    {
        _decoder.ActorController.SetActiveSpeaker(actor);
        _decoder.ActorController.SetSpeakingType(speakingType);
        OnActionDone?.Invoke();
    }

    private void SET_POSE(string poseName, string optional_targetActor = null)
    {
        if (optional_targetActor == null)
        {
            _decoder.ActorController.SetPose(poseName);
            OnActionDone?.Invoke();
        }
        else
        {
            _decoder.ActorController.SetPose(poseName, optional_targetActor);
            OnActionDone?.Invoke();
        }
    }

    private void PLAY_EMOTION(string poseName, string optional_targetActor = null)
    {
        if (optional_targetActor == null)
        {
            _decoder.ActorController.PlayEmotion(poseName);
        }
        else
        {
            _decoder.ActorController.PlayEmotion(poseName, optional_targetActor);
        }
    }

    private void SET_ACTOR_POSITION(int oneBasedSlotIndex, string actorName)
    {
        _decoder.ActorController.AssignActorToSlot(actorName, oneBasedSlotIndex);
        OnActionDone?.Invoke();
    }
    #endregion
#pragma warning restore IDE0051 // Remove unused private members
    // ReSharper restore UnusedMember.Local
    // ReSharper restore InconsistentNaming
}
