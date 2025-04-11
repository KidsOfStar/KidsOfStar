using MainTable;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : ISceneLifecycleHandler
{
    private readonly Dictionary<DialogActionType, IDialogActionHandler> dialogActionHandlers = new();
    private readonly Dictionary<CharacterType, NPC> npcDictionary = new();
    private readonly Queue<string> dialogQueue = new();
    
    private PlayerData currentDialogData;
    private UITextBubble textBubble;
    public Action OnClick { get; set; }

    public DialogueManager()
    {
        dialogActionHandlers[DialogActionType.None] = new NoneAction();
        dialogActionHandlers[DialogActionType.ShowSelect] = new ShowSelectAction();
        dialogActionHandlers[DialogActionType.ModifyTrust] = new ModifyTrustAction();
        dialogActionHandlers[DialogActionType.DataSave] = new DataSaveAction();
    }
    
    public void InitNPcs(NPC[] npcs)
    {
        foreach (var npc in npcs)
        {
            npcDictionary[npc.CharacterType] = npc;
        }
    }

    // 현재 출력 할 대사 데이터를 초기화
    public void SetCurrentDialogData(int index)
    {
        currentDialogData = Managers.Instance.DataManager.GetPlayerData(index);
        if (currentDialogData == null)
        {
            EditorLog.LogError($"DialogueManager : Not found PlayerData with index: {index}");
            return;
        }
        
        // 데이터의 대사 value 값을 @로 나누어 대사 큐에 넣음 
        var dialogs = currentDialogData.DialogValue.Split('@');
        foreach (var dialog in dialogs)
            dialogQueue.Enqueue(dialog);

        if (dialogQueue.Count > 0)
        {
            ShowDialog(dialogQueue.Dequeue(), currentDialogData.Character);
        }
    }

    // 한 줄씩 대사를 출력
    public void ShowDialog(string dialog, CharacterType character)
    {
        var bubblePos = npcDictionary[character].BubbleOffset;
        var localPos = WorldToCanvasPosition(bubblePos);

        textBubble.SetActive(true);
        textBubble.SetDialog(dialog, localPos);
    }

    // 한 라인이 끝났는지?
    public void OnDialogLineComplete()
    {
        if (dialogQueue.Count > 0)
        {
            ShowDialog(dialogQueue.Dequeue(), currentDialogData.Character);
        }
        else
        {
            textBubble.HideDirect();
            
            // 타입에 따라 다이얼로그 액션 실행
            dialogActionHandlers[currentDialogData.DialogType].Execute(currentDialogData);
        }
    }

    private Vector2 WorldToCanvasPosition(Vector3 worldPos)
    {
        Vector3 screenPos = Managers.Instance.GameManager.MainCamera.WorldToScreenPoint(worldPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
                                                                Managers.Instance.UIManager.CanvasRectTr,
                                                                screenPos,
                                                                null,
                                                                out var localPos
                                                               );
        return localPos;
    }
    
    // None
    /// nextDialog가 없을 때까지 출력해야 함
    // ShowSelect
    /// 대사 출력이 끝나고 선택지를 보여줘야 함
    // DataSave
    /// 대사 출력이 끝나고 데이터를 세이브할지 말지 결정해야함
    // ModifyTrust
    /// 대사 출력이 끝나고 신뢰도를 수정해야함

    // 대사가 끝나면 액션에 따라 할 일이 달라짐
    // Enum 타입으로 가지고 있음
    public void OnSceneLoaded()
    {
        textBubble = Managers.Instance.UIManager.Show<UITextBubble>();
        textBubble.HideDirect();
    }

    public void OnSceneUnloaded()
    {
        npcDictionary.Clear();
    }
}