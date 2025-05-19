using MainTable;
using System.Collections.Generic;
using UnityEngine;

public class UISelectionPanel : UIBase
{
    [SerializeField] private UISelectButton[] selectButtons;
    private DialogData dialogData;
    private List<int> finalNextIndexes;
    
    public void SetDefaultPanel(DialogData dialog)
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

        Managers.Instance.SoundManager.PlaySfx(SfxSoundType.ImportantChoice);
        for (int i = 0; i < selectionList.Count; i++)
            selectButtons[i].HighlightInit(i, OnSelectButtonClick, dialogData.SelectOption[i]);
    }

    public void SetFinalPanel(List<string> finalSelection, List<int> nextIndexes)
    {
        finalNextIndexes = nextIndexes;
        
        Managers.Instance.SoundManager.PlaySfx(SfxSoundType.ImportantChoice);
        for (int i = 0; i < finalSelection.Count; i++)
        {
            selectButtons[i].HighlightInit(i, OnSelectButtonClick, finalSelection[i]);
        }
    }

    private void OnSelectButtonClick(int index)
    {
        int nextIndex = finalNextIndexes != null ? finalNextIndexes[index] : dialogData.NextIndex[index];
        
        HideDirect();
        if (nextIndex < 0)
        {
            // 데이터 매니저에서 특수인덱스 가져오기
            var specifiedAction = Managers.Instance.DataManager.GetSpecifiedActionData(nextIndex);
            // 다이얼로그 매니저에서 특수인덱스 실행하기
            CustomActions.ExecuteAction(specifiedAction);
            return;
        }
        
        Managers.Instance.SoundManager.PlaySfx(SfxSoundType.ButtonPush);
        Managers.Instance.DialogueManager.SetCurrentDialogData(nextIndex);
        
        // 특정 index 선택지일떄만 발생하게 할 수는 있음
        // 이벤트 이름 : select Info < 선택지를 선택했을 때 항상 발생
        // 현재 다이얼로그 index : 10022
        // 선택한 선택지 : 좋아, 싫어
    }

    private void OnDisable()
    {
        for (int i = 0; i < selectButtons.Length; i++)
        {
            selectButtons[i].gameObject.SetActive(false);
        }
    }
}
