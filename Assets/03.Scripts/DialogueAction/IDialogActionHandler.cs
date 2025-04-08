using MainTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDialogActionHandler
{
    // TODO: PlayerData -> DialogData
    void Execute(PlayerData playerData);
}

public class NoneAction : IDialogActionHandler
{
    public void Execute(PlayerData playerData)
    {
        // Do nothing
    }
}