using MainTable;

public class DataSaveAction : IDialogActionHandler
{
    public void Execute(DialogData playerData, bool isFirst)
    {
        if (!isFirst)
        {
            Managers.Instance.DialogueManager.OnDialogEnd?.Invoke();
            Managers.Instance.DialogueManager.InvokeSceneDialogEnd();
        }
        
        // TODO: Save UI 띄우기
    }
}
