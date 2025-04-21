using MainTable;
using System;

public class LoadSceneAction : IDialogActionHandler
{
    public void Execute(DialogData dialogData)
    {
        // 대화가 끝났음을 알리는 콜백
        Managers.Instance.DialogueManager.OnDialogEnd?.Invoke();
        Managers.Instance.DialogueManager.InvokeSceneDialogEnd();
        
        if (!Enum.TryParse<SceneType>(dialogData.Param, out var sceneType))
        {
            EditorLog.LogError($"LoadCutSceneAction : Invalid load scene type: {dialogData.Param}");
            return;
        }

        Managers.Instance.SceneLoadManager.LoadScene(sceneType);
    }
}