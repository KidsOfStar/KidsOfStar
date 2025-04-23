using MainTable;

public class ChangeFormAction : IDialogActionHandler
{
    public void Execute(DialogData dialogData, bool isFirst)
    {
        if (!isFirst)
        {
            Managers.Instance.DialogueManager.OnDialogEnd?.Invoke();
            Managers.Instance.DialogueManager.InvokeSceneDialogEnd();
        }

        var player = Managers.Instance.GameManager.Player;
        player.FormControl.FormChange(dialogData.Param);
    }
}