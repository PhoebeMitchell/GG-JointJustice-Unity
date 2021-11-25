using Ink.Runtime;
using UnityEngine;

public class ScriptDecoder 
{
    private ActionDecoder _actionDecoder;

    public ScriptDecoder(IDecoder decoder, TextAsset script)
    {
        _actionDecoder = new ActionDecoder(decoder);
        var story = new Story(script.text);
        while (story.canContinue)
        {
            string line = story.Continue();
            if (line[0] == '&')
            {
                _actionDecoder.OnNewActionLine(line);
            }
        }
    }
}
