using MainTable;
using UnityEngine;

public class UISelectionPanel : PopupBase
{
    [SerializeField] private UISelectButton[] selectButtons;
    private DialogData dialogData;
    
    public void SetPanel(DialogData dialog)
    {
        dialogData = dialog;
        var selectionList = dialogData.SelectOption;
        
        for (int i = 0; i < selectionList.Count; i++)
            selectButtons[i].DefaultInit(i, OnSelectButtonClick, dialogData.SelectOption[i]);
    }

    public void SetHighlightPanel(DialogData dialog)
    {
        dialogData = dialog;
        var selectionList = dialogData.SelectOption;

        for (int i = 0; i < selectionList.Count; i++)
            selectButtons[i].HighlightInit(i, OnSelectButtonClick, dialogData.SelectOption[i]);
    }

    private void OnSelectButtonClick(int index)
    {
        var nextIndex = dialogData.NextIndex[index];
        HideDirect();
        if (nextIndex < 0)
        {
            // 데이터 매니저에서 특수인덱스 가져오기
            var specifiedAction = Managers.Instance.DataManager.GetSpecifiedActionData(nextIndex);
            // 다이얼로그 매니저에서 특수인덱스 실행하기
            CustomActions.ExecuteAction(specifiedAction);
            return;
        }
        
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
