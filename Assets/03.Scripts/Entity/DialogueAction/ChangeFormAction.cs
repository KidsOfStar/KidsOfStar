using MainTable;
using System;

public class ChangeFormAction : IDialogActionHandler
{
    // NextIndex 존재 불가능
    public void Execute(DialogData dialogData, bool isFirst)
    {
        if (!isFirst)
        {
            Managers.Instance.DialogueManager.InvokeOnDialogStepEnd();
            Managers.Instance.DialogueManager.OnDialogEnd?.Invoke();
        }

        var player = Managers.Instance.GameManager.Player;
        if (!player.FormControl.GetFormLock(dialogData.Param))
            player.FormControl.SetFormActive(dialogData.Param);

        player.FormControl.FormChange(dialogData.Param);

        if (!Enum.TryParse<PlayerFormType>(dialogData.Param, out var formType))
        {
            EditorLog.LogError($"ChangeFormAction : Invalid form type : {dialogData.Param}");
            return;
        }
        
        Managers.Instance.GameManager.UnlockForm(formType);
    }
}