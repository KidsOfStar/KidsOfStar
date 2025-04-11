using MainTable;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UISelectionPanel : UIBase
{
    [SerializeField] private UISelectButton[] selectButtons;
    private PlayerData dialogData;
    private const string SelectButtonKey = "UISelectButton";
    
    public void SetPanel(PlayerData dialog)
    {
        dialogData = dialog;
        var selectionList = dialogData.SelectOption;
        
        for (int i = 0; i < selectionList.Count; i++)
        {
            selectButtons[i].Init(i, OnSelectButtonClick, dialogData.SelectOption[i]);
            selectButtons[i].gameObject.SetActive(true);
        }
    }

    private void OnSelectButtonClick(int index)
    {
        var nextIndex = dialogData.NextIndex[index];
        HideDirect();
        Managers.Instance.DialogueManager.SetCurrentDialogData(nextIndex);
    }

    private void OnDisable()
    {
        for (int i = 0; i < selectButtons.Length; i++)
        {
            selectButtons[i].gameObject.SetActive(false);
        }
    }
}
