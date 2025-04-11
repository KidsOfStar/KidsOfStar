using MainTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyTrustAction : IDialogActionHandler
{
    public void Execute(DialogData playerData)
    {
        // TODO
        // Managers.Instance.GameManager.ModifyTrust(playerData.TrustValue);
        
        if (playerData.NextIndex.Count <= 0)
        {
            Managers.Instance.DialogueManager.OnDialogEnd?.Invoke();
            return;
        }

        var nextIndex = playerData.NextIndex[0];
        Managers.Instance.DialogueManager.SetCurrentDialogData(nextIndex);
    }
}