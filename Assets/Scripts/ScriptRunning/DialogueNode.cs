public class DialogueNode : INode
{
    private string _dialogueLine;
    private IAppearingDialogueController _appearingDialogueController;

    public INode NextNode { get; set; }

    public DialogueNode(string dialogueLine, IAppearingDialogueController appearingDialogueController)
    {
        _dialogueLine = dialogueLine;
        _appearingDialogueController = appearingDialogueController;
    }
    
    public void Execute()
    {
        _appearingDialogueController.StartDialog(_dialogueLine);
    }
}
