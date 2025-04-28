using MainTable;
using System;

// NExt
public class LoadSceneAction : IDialogActionHandler
{
    public void Execute(DialogData dialogData, bool isFirst)
    {
        if (!isFirst)
        {
            Managers.Instance.DialogueManager.InvokeOnDialogStepEnd();
            Managers.Instance.DialogueManager.OnDialogEnd?.Invoke();
        }
        
        if (!Enum.TryParse<SceneType>(dialogData.Param, out var sceneType))
        {
            EditorLog.LogError($"LoadSceneAction : Invalid load scene type: {dialogData.Param}");
            return;
        }

        Managers.Instance.SceneLoadManager.LoadScene(sceneType);
    }
}