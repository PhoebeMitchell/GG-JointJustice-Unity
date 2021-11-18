using System;

public struct ScriptAction
{
    public ScriptAction(String line)
    {
        Text = line;
    }
    
    public string Text { get; set; }
    // public Action Action { get; set; }
}
