using MainTable;

public class UpdateProgressAction : IDialogActionHandler
{
    public void Execute(DialogData dialogData)
    {
        // 대화가 끝났음을 알리는 콜백
        Managers.Instance.DialogueManager.OnDialogEnd?.Invoke();
        Managers.Instance.DialogueManager.InvokeSceneDialogEnd();
        
        Managers.Instance.GameManager.UpdateProgress();
    }
}
