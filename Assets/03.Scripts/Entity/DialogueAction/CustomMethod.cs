using System;
using System.Collections.Generic;

public class CustomDialogMethod
{
    private readonly Dictionary<DialogCustomMethodType, Action> methodMap = new();
    
    public CustomDialogMethod()
    {
        methodMap.Add(DialogCustomMethodType.ChangeHumanForm, ChangeHumanForm);
    }
    
    private void ChangeHumanForm()
    {
        // 대화 종료 콜백
        Managers.Instance.DialogueManager.OnDialogEnd?.Invoke();
        Managers.Instance.DialogueManager.InvokeSceneDialogEnd();
        
        Managers.Instance.GameManager.Player.FormControl.FormChange("Human");
        Managers.Instance.GameManager.UpdateProgress();
    }
    
    public void Invoke(DialogCustomMethodType methodType)
    {
        if (methodMap.TryGetValue(methodType, out var action))
            action.Invoke();
    }
}
