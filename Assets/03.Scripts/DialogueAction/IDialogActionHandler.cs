using MainTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDialogActionHandler
{
    // TODO: PlayerData -> DialogData
    void Execute(DialogData playerData);
}

public class NoneAction : IDialogActionHandler
{
    public void Execute(DialogData playerData)
    {
        if (playerData.NextIndex.Count <= 0)
        {
            Managers.Instance.DialogueManager.OnDialogEnd?.Invoke();
            return;
        }

        var nextIndex = playerData.NextIndex[0];
        Managers.Instance.DialogueManager.SetCurrentDialogData(nextIndex);
    }
}