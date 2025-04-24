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
        EditorLog.Log("ChangeFormAction : " + dialogData.Param);
        
        // if (player.FormControl.)
        player.FormControl.FormChange(dialogData.Param);
    }
}