using MainTable;
using System.Collections.Generic;
using UnityEngine;

public class UISelectionPanel : UIBase
{
    private PlayerData dialogData;
    private const string SelectButtonKey = "UISelectButton";
    
    public void SetPanel(PlayerData dialog)
    {
        dialogData = dialog;
        var selectionList = dialogData.SelectOption;
        for (int i = 0; i < selectionList.Count; i++)
        {
            var select = Managers.Instance.PoolManager.Spawn<UISelectButton>(SelectButtonKey, transform);
            select.Init(i, OnSelectButtonClick, dialogData.SelectOption[i]);
        }
    }

    private void OnSelectButtonClick(int index)
    {
        var nextIndex = dialogData.NextIndex[index];
        HideDirect();
        Managers.Instance.DialogueManager.SetCurrentDialogData(nextIndex);
    }
}
