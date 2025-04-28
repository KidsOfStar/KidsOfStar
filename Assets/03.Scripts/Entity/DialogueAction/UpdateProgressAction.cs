using MainTable;

public class UpdateProgressAction : IDialogActionHandler
{
    public void Execute(DialogData dialogData, bool isFirst)
    {
        if (!isFirst)
        {
            Managers.Instance.DialogueManager.InvokeSceneDialogEnd();
            Managers.Instance.DialogueManager.OnDialogEnd?.Invoke();
        }
        
        Managers.Instance.GameManager.UpdateProgress();
    }
}
