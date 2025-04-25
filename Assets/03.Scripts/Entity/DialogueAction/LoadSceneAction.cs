using MainTable;
using System;

public class LoadSceneAction : IDialogActionHandler
{
    public void Execute(DialogData dialogData, bool isFirst)
    {
        if (!isFirst)
        {
            Managers.Instance.DialogueManager.OnDialogEnd?.Invoke();
            Managers.Instance.DialogueManager.InvokeSceneDialogEnd();
        }
        
        if (!Enum.TryParse<SceneType>(dialogData.Param, out var sceneType))
        {
            EditorLog.LogError($"LoadSceneAction : Invalid load scene type: {dialogData.Param}");
            return;
        }

        Managers.Instance.SceneLoadManager.LoadScene(sceneType);
    }
}