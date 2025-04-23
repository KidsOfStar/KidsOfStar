using MainTable;

public class ModifyTrustAction : IDialogActionHandler
{
    public void Execute(DialogData dialogData, bool isFirst)
    {
        var trustValue = int.Parse(dialogData.Param);
        Managers.Instance.GameManager.ModifyTrust(trustValue);

        // 첫번째 액션이라면 return
        if (isFirst) return;
        
        if (dialogData.NextIndex.Count <= 0)
        {
            Managers.Instance.DialogueManager.OnDialogEnd?.Invoke();
            Managers.Instance.DialogueManager.InvokeSceneDialogEnd();
            return;
        }

        var nextIndex = dialogData.NextIndex[0];
        Managers.Instance.DialogueManager.SetCurrentDialogData(nextIndex);
    }
}