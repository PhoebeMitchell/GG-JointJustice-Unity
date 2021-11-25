using System.Collections.Generic;
using Ink.Runtime;

public class ScriptReader 
{
    private static List<string> _navigatedPaths = new List<string>();
    
    public ScriptReader(IDecoder decoder, Story story)
    {
        if (_navigatedPaths.Contains(story.state.currentPathString))
            return;
        
        _navigatedPaths.Add(story.state.currentPathString);
        
        ActionDecoder actionDecoder = new ActionDecoder(decoder);
        while (story.canContinue)
        {
            string line = story.Continue();
            if (line[0] == '&')
            {
                actionDecoder.OnNewActionLine(line);
            }
        }

        int choiceCount = story.currentChoices.Count;
        if (choiceCount > 0)
        {
            string storyText = story.ToJson();
            string storyState = story.state.ToJson();
            for (int i = 0; i < choiceCount; i++)
            {
                Story storyDuplicate = new Story(storyText);
                storyDuplicate.state.LoadJson(storyState);
                storyDuplicate.ChooseChoiceIndex(i);
               
                ScriptReader scriptReader = new ScriptReader(decoder, storyDuplicate);
            }
        }
    }
}
