using MainTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDialogActionHandler
{
    void Execute(DialogData dialogData);
}

public class NoneAction : IDialogActionHandler
{
    public void Execute(DialogData dialogData)
    {
        if (dialogData.NextIndex.Count <= 0)
        {
            Managers.Instance.DialogueManager.OnDialogEnd?.Invoke();
            return;
        }

        var nextIndex = dialogData.NextIndex[0];
        Managers.Instance.DialogueManager.SetCurrentDialogData(nextIndex);
    }
}