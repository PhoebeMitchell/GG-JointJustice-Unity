using System.Reflection;

public class ActionNode : INode
{
    private MethodInfo _methodInfo;
    private object[] _parameters;
    private IDecoder _decoder;

    public INode NextNode { get; set; }
    
    public ActionNode(MethodInfo methodInfo, object[] parameters, IDecoder decoder)
    {
        _methodInfo = methodInfo;
        _parameters = parameters;
        _decoder = decoder;
    }
    
    public void Execute()
    {
        // _methodInfo.Invoke(_decoder, _parameters);
    }
}
