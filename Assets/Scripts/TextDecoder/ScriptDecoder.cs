using System.Collections.Generic;
using System.Reflection;
using Ink.Runtime;
using UnityEngine;

public class ScriptDecoder 
{
    private Queue<INode> _nodes = new Queue<INode>();
    private ActionDecoder _actionDecoder;

    public ScriptDecoder(IDecoder decoder, TextAsset script)
    {
        _actionDecoder = new ActionDecoder(decoder);
        var story = new Story(script.text);
        while (story.canContinue)
        {
            string line = story.Continue();
            INode node = line[0] == '&'
                ? _actionDecoder.Decode(line)
                : (INode)new DialogueNode(line, decoder.AppearingDialogueController);
            node.Execute();
            _nodes.Enqueue(node);
        }
    }
    
    public void NextAction()
    {
        _nodes.Dequeue().Execute();
    }
}
