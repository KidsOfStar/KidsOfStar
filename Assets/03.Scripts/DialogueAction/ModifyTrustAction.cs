using MainTable;

public class ModifyTrustAction : IDialogActionHandler
{
    public void Execute(DialogData playerData)
    {
       // Managers.Instance.GameManager.ModifyTrust(playerData.Param);
        
        if (playerData.NextIndex.Count <= 0)
        {
            Managers.Instance.DialogueManager.OnDialogEnd?.Invoke();
            return;
        }

        var nextIndex = playerData.NextIndex[0];
        Managers.Instance.DialogueManager.SetCurrentDialogData(nextIndex);
    }
}