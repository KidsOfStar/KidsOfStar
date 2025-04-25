using System.Collections.Generic;
using UnityEngine;

public abstract class InteractSpeaker : MonoBehaviour
{
    [field: SerializeField] public CharacterType CharacterType { get; private set; }
    private readonly Dictionary<int, int> dialogByProgress = new();
    private readonly Dictionary<int, int> requiredDialogByProgress = new();

    public void Init()
    {
        var dict = Managers.Instance.DataManager.GetNpcDataDict();
        var startRange = ((int)Managers.Instance.GameManager.CurrentChapter + 1) * 100;
        var endRange = startRange + 100;

        for (int i = startRange; i < endRange; i++)
        {
            if (!dict.TryGetValue(i, out var npcData)) break;
            if (npcData.Character != CharacterType) continue;

            dialogByProgress.Add(npcData.Progress, npcData.DialogIndex);
        }

        //InitRequiredDialog();
        
        // 게임매니저의 업데이트 프로그레스 이벤트에 현재 프로그레스에 맞는 필수대사가 있다면 느낌표를 띄우는 함수 등록
        // 다이얼로그 매니저의 특정 대사 End 이벤트에 느낌표를 제거하는 함수 등록
    }
    
    private void InitRequiredDialog()
    {
        // 필수 대사 인덱스
        var data = Managers.Instance.ResourceManager.Load<RequiredIndexData>(Define.dataPath + Define.requiredIndex);
        var indexes = data.requiredIndexList;

        for (int i = 0; i < indexes.Length; i++)
        {
            var indexData = indexes[i];
            
            // 자신의 캐릭터 타입과 맞다면 딕셔너리에 저장
            if (indexData.characterType == CharacterType)
                requiredDialogByProgress[indexData.progress] = indexData.index;
        }
    }
    
    // 현재 프로그레스에 맞는 인덱스가 있다면? -> 느낌표 띄우기
    // 해당 인덱스의 대사가 출력됐다면? -> 느낌표 삭제
    private bool ExistRequiredDialog(int progress)
    {
        var currentProgress = Managers.Instance.GameManager.ChapterProgress;
        foreach (var pair in requiredDialogByProgress)
        {
            
        }
        
        if (currentProgress == progress)
        {
            return true;
        }

        return false;
    }
    

    private void OnInteract()
    {
        // 상호작용 버튼 이벤트에 해제
        var skillPanel = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel;
        skillPanel.OnInteractBtnClick -= OnInteract;

        // 대사 종료 이벤트에 등록
        Managers.Instance.DialogueManager.OnDialogEnd -= HideInteractionButton; 
        Managers.Instance.DialogueManager.OnDialogEnd += HideInteractionButton;
        
        // 대사 출력
        ShowDialog();
    }

    private void ShowDialog()
    {
        var key = Managers.Instance.GameManager.ChapterProgress;
        if (!dialogByProgress.TryGetValue(key, out int dialogIndex))
        {
            Debug.LogWarning($"No dialog found for progress {key}");
            return;
        }

        Managers.Instance.DialogueManager.SetCurrentDialogData(dialogIndex);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 상호작용 버튼 이벤트에 등록
        if (!other.CompareTag("Player")) return;

        var skillPanel = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel;
        skillPanel.OnInteractBtnClick -= OnInteract;
        skillPanel.OnInteractBtnClick += OnInteract;
        skillPanel.ShowInteractionButton(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 상호작용 버튼 이벤트에 해제
        if (!other.CompareTag("Player")) return;

        var skillPanel = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel;
        skillPanel.ShowInteractionButton(false);
        skillPanel.OnInteractBtnClick -= OnInteract;
        Managers.Instance.DialogueManager.OnDialogEnd -= HideInteractionButton;
    }

    private void HideInteractionButton()
    {
        var skillPanel = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel;
        skillPanel.ShowInteractionButton(false);
    }
    
    
}