using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;

public class ScriptDecoder
{
    private Story _story;
    private Queue<ScriptAction> _actions = new Queue<ScriptAction>();
    
    public ScriptDecoder(TextAsset script)
    {
        _story = new Story(script.text);
        while (_story.canContinue)
        {
            _actions.Enqueue(new ScriptAction(_story.Continue()));
        }

        while (_actions.Count > 0)
        {
            Debug.Log(_actions.Dequeue().Text);
        }
    }
}
