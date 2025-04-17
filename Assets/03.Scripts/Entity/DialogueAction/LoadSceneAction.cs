using MainTable;
using System;

public class LoadSceneAction : IDialogActionHandler
{
    public void Execute(DialogData dialogData)
    {
        if (!Enum.TryParse<SceneType>(dialogData.Param, out var sceneType))
        {
            EditorLog.LogError($"PlayCutSceneAction : Invalid cutscene type: {dialogData.Param}");
            return;
        }

        Managers.Instance.SceneLoadManager.LoadScene(sceneType);
    }
}