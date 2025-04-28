using MainTable;

public class DataSaveAction : IDialogActionHandler
{
    public void Execute(DialogData playerData, bool isFirst)
    {
        if (!isFirst)
        {
            Managers.Instance.DialogueManager.InvokeSceneDialogEnd();
            Managers.Instance.DialogueManager.OnDialogEnd?.Invoke();
        }
        
        // TODO: Save UI 띄우기
    }
}
