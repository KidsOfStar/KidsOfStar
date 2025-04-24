using MainTable;
using System;

public class GoToEndingAction : IDialogActionHandler
{
    public void Execute(DialogData dialogData, bool isFirst)
    {
        if (!isFirst)
        {
            Managers.Instance.DialogueManager.OnDialogEnd?.Invoke();
            Managers.Instance.DialogueManager.InvokeSceneDialogEnd();
        }

        if (!Enum.TryParse<EndingType>(dialogData.Param, out var endingType))
        {
            EditorLog.LogError($"GoToEndingAction : Invalid load scene type: {dialogData.Param}");
            return;
        }
        
        Managers.Instance.GameManager.TriggerEnding(endingType);
    }
}
