using System.Reflection;

public struct ScriptAction
{
    public ScriptAction(MethodInfo info, object[] parameters, string text = "")
    {
        Info = info;
        Parameters = parameters;
        Text = text;
    }

    public MethodInfo Info { get; }
    public object[] Parameters { get; }
    public string Text { get; }
    
    public override string ToString()
    {
        return Text == "" ? Info.Name : Text;
    }
}
