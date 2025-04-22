using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractSpeaker : MonoBehaviour
{
    [field: SerializeField] public CharacterType CharacterType { get; private set; }
    private Dictionary<int, int> DialogByProgress { get; set; } = new();

    private void Start()
    {
        var dict = Managers.Instance.DataManager.GetNpcDataDict();
        var startRange = ((int)Managers.Instance.GameManager.CurrentChapter + 1) * 100;
        var endRange = startRange + 100;

        for (int i = startRange; i < endRange; i++)
        {
            if (!dict.TryGetValue(i, out var npcData)) break; 
            if (npcData.Character != CharacterType) continue;
            
            DialogByProgress.Add(npcData.Progress, npcData.DialogIndex);
        }
    }

    private void OnInteract()
    {
        // 상호작용 버튼 이벤트에 해제
        var skillPanel = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel;
        skillPanel.OnInteractBtnClick -= OnInteract;
        
        // 대사 출력
        var key = Managers.Instance.GameManager.ChapterProgress;
        var dialogIndex = DialogByProgress[key];
        Managers.Instance.DialogueManager.SetCurrentDialogData(dialogIndex);
        
        // 다시 상호작용 할 수 있도록 대화가 끝나면 다시 이벤트에 등록
        Managers.Instance.DialogueManager.OnDialogEnd -= AddListenerOnInteract;
        Managers.Instance.DialogueManager.OnDialogEnd += AddListenerOnInteract;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 상호작용 버튼 이벤트에 등록
        if (!other.CompareTag("Player")) return;

        var skillPanel = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel;
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
        Managers.Instance.DialogueManager.OnDialogEnd -= AddListenerOnInteract;
    }
    
    private void AddListenerOnInteract()
    {
        var skillPanel = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel;
        skillPanel.OnInteractBtnClick += OnInteract;
    }
}