using MainTable;

public class UpdateProgressAction : IDialogActionHandler
{
    public void Execute(DialogData dialogData, bool isFirst)
    {
        if (!isFirst)
        {
            Managers.Instance.DialogueManager.OnDialogEnd?.Invoke();
            Managers.Instance.DialogueManager.InvokeSceneDialogEnd();
        }
        
        EditorLog.Log("UpdateProgressAction");
        Managers.Instance.GameManager.UpdateProgress();
    }
}
