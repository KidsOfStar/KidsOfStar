using MainTable;
using System;

// 대화 종료 콜백을 호출해야 한다면 커스텀 메서드 안에서 호출해야 한다.
public class ExecuteCustomAction : IDialogActionHandler
{
    public void Execute(DialogData dialogData)
    {
        if (!Enum.TryParse<DialogCustomMethodType>(dialogData.Param, out var method))
        {
            EditorLog.LogError($"ExecuteCustomAction : Invalid custom dialog method: {dialogData.Param}");
            return;
        }
        
        Managers.Instance.DialogueManager.CustomDialogMethod?.Invoke(method);
    }
}
