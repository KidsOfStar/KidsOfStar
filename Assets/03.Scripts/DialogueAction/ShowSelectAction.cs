using MainTable;

public class ShowSelectAction : IDialogActionHandler
{
    public void Execute(PlayerData playerData)
    {
        var selectionPanel = Managers.Instance.UIManager.Show<UISelectionPanel>();
        selectionPanel.SetPanel(playerData);
    }
}
