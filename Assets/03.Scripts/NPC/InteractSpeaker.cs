using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractSpeaker : MonoBehaviour
{
    [field: SerializeField] public CharacterType CharacterType { get; private set; }
    private readonly Dictionary<int, int> dialogByProgress = new();

    private void Start()
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
    
    // TODO: 느낌표 마크 풀 생성
    
    // Data 읽어와서 가지고 있기 / 프로그레스 이벤트 Invoke
    
    // 특정 대사 인덱스 - 프로그레스 조합
    
    // 해당 인덱스의 대사가 출력됐다면? -> 느낌표 삭제
}