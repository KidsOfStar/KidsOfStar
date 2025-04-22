using System;
using System.Collections.Generic;

public class CustomDialogMethod
{
    private readonly Dictionary<DialogCustomMethodType, Action> methodMap = new();
    
    public CustomDialogMethod()
    {
        methodMap.Add(DialogCustomMethodType.ChangeHumanForm, ChangeHumanForm);
    }
    
    public void ChangeHumanForm()
    {
        // 대화 종료 콜백
        Managers.Instance.DialogueManager.OnDialogEnd?.Invoke();
        Managers.Instance.DialogueManager.InvokeSceneDialogEnd();
        
        // TODO: 플레이어 강제로 사람 폼으로 변경
        Managers.Instance.GameManager.UpdateProgress();
    }
    
    public void Invoke(DialogCustomMethodType methodType)
    {
        if (methodMap.TryGetValue(methodType, out var action))
            action.Invoke();
    }
}
