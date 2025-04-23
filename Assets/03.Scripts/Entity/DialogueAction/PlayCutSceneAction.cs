using MainTable;
using System;

public class PlayCutSceneAction : IDialogActionHandler
{
    public void Execute(DialogData dialogData, bool isFirst)
    {
        if (!isFirst)
        {
            Managers.Instance.DialogueManager.OnDialogEnd?.Invoke();
            Managers.Instance.DialogueManager.InvokeSceneDialogEnd();
        }
        
        if (!Enum.TryParse<CutSceneType>(dialogData.Param, out var cutScene))
        {
            EditorLog.LogError($"PlayCutSceneAction : Invalid cutscene type: {dialogData.Param}");
            return;
        }

        Managers.Instance.CutSceneManager.PlayCutScene(cutScene.GetName());
    }
}
