using MainTable;
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class DialogueManager : ISceneLifecycleHandler
{
    private Dictionary<CharacterType, NPC> npcDictionary = new();
    private Transform maorum; // 임시
    private UITextBubble textBubble;

    public Action OnClick;
    private PlayerData currentDialogData;
    private Queue<string> dialogQueue = new();

    // TODO: 씬 로드시마다 NPC를 가져와야함
    // TODO: 씬에 진입하면 씬에 존재하는 NPC들을 가져옴
    // npc를 가지고 있는 딕셔너리

    public void InitNPcs(NPC[] npcs)
    {
        foreach (var npc in npcs)
        {
            npcDictionary[npc.CharacterType] = npc;
        }
    }

    public void SetCurrentDialogData(int index, Vector3 offset)
    {
        currentDialogData = Managers.Instance.DataManager.GetPlayerData(index);
        var dialogs = currentDialogData.DialogValue.Split('@');
        foreach (var dialog in dialogs)
            dialogQueue.Enqueue(dialog);

        if (dialogQueue.Count > 0)
        {
            ShowDialog(dialogQueue.Dequeue(), offset);
        }
    }

    public void ShowDialog(string dialog, Vector3 offset)
    {
        var localPos = WorldToCanvasPosition(maorum.position + offset);

        textBubble.SetActive(true);
        textBubble.SetDialog(dialog, localPos);
    }

    // 한 라인이 끝났는지?
    // 아니면 다음 인덱스로 넘어가는지?
    public void OnDialogLineComplete()
    {
        if (dialogQueue.Count > 0)
        {
            ShowDialog(dialogQueue.Dequeue(), Vector3.zero);
        }
        else
        {
            textBubble.HideDirect();
            Managers.Instance.DialogInputHandler.gameObject.SetActive(false);
            OnClick?.Invoke();
        }
    }

    private Vector2 WorldToCanvasPosition(Vector3 worldPos)
    {
        // Vector3 screenPos = Managers.Instance.GameManager.MainCamera.WorldToScreenPoint(worldPos);
        // RectTransformUtility.ScreenPointToLocalPointInRectangle(
        //                                                         Managers.Instance.UIManager.CanvasRectTr,
        //                                                         screenPos,
        //                                                         null,
        //                                                         out var localPos
        //                                                        );
        // return localPos;
        return Vector2.zero;
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

        // 임시
        // maorum = Object.FindObjectOfType<NPC>().transform;
        // Managers.Instance.DialogInputHandler.gameObject.SetActive(true);
    }

    public void OnSceneUnloaded()
    {
        npcDictionary.Clear();
    }
}