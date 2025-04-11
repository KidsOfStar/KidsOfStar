using MainTable;

public class ShowSelectAction : IDialogActionHandler
{
    public void Execute(DialogData dialogData)
    {
        var selectionPanel = Managers.Instance.UIManager.Show<UISelectionPanel>();
        selectionPanel.SetPanel(dialogData);
    }
}
