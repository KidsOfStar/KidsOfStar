using MainTable;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : ISceneLifecycleHandler
{
    private readonly Dictionary<DialogActionType, IDialogActionHandler> dialogActionHandlers = new();
    private readonly Dictionary<CharacterType, IDialogSpeaker> sceneSpeakers = new();
    private readonly Dictionary<CharacterType, IDialogSpeaker> cutSceneSpeakers = new();
    private readonly Queue<string> dialogQueue = new();

    private DialogData currentDialogData;
    private UITextBubble textBubble;

    public Action OnClick { get; set; }
    public Action OnDialogStart { get; set; }
    public Action OnDialogEnd { get; set; }
    private bool isCutScene;

    public DialogueManager()
    {
        dialogActionHandlers[DialogActionType.None] = new NoneAction();
        dialogActionHandlers[DialogActionType.ShowSelect] = new ShowSelectAction();
        dialogActionHandlers[DialogActionType.ModifyTrust] = new ModifyTrustAction();
        dialogActionHandlers[DialogActionType.DataSave] = new DataSaveAction();
        dialogActionHandlers[DialogActionType.PlayCutScene] = new PlayCutSceneAction();
        dialogActionHandlers[DialogActionType.LoadScene] = new LoadSceneAction();
    }

    public void InitSceneNPcs(Npc[] speakers)
    {
        // 대사를 말할 수 있는 기능을 인터페이스로 빼야겠는데.
        foreach (var npc in speakers)
        {
            sceneSpeakers[npc.GetCharacterType()] = npc;
        }
    }

    public void InitCutSceneNPcs(Npc[] speakers)
    {
        foreach (var npc in speakers)
        {
            cutSceneSpeakers[npc.GetCharacterType()] = npc;
        }
    }

    public void SetPlayerSpeaker(IDialogSpeaker player)
    {
        sceneSpeakers[CharacterType.Dolmengee] = player;
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

        // 인덱스가 10000 미만이면 컷씬으로 판단
        isCutScene = currentDialogData.Index < 10000;

        // 데이터의 대사 value 값을 @로 나누어 대사 큐에 넣음 
        var dialogs = currentDialogData.DialogValue.Split('@');
        foreach (var dialog in dialogs)
            dialogQueue.Enqueue(dialog);

        if (dialogQueue.Count > 0)
        {
            ShowDialog(dialogQueue.Dequeue(), currentDialogData.Character);
        }
    }

    private void ShowDialog(string dialog, CharacterType character)
    {
        var npc = isCutScene ? cutSceneSpeakers[character] : sceneSpeakers[character];
        Vector3 bubblePos = npc.GetBubblePosition();

        var localPos = WorldToCanvasPosition(bubblePos);
        var formattedDialog = dialog.Replace("\\n", "\n");

        OnDialogStart?.Invoke();
        textBubble.SetActive(true);
        textBubble.SetDialog(formattedDialog, localPos);
    }

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
        var cam = Managers.Instance.GameManager.MainCamera;
#if UNITY_EDITOR
        if (Managers.Instance.IsDebugMode && !cam)
        {
            cam = Camera.main;
            Managers.Instance.GameManager.SetCamera(cam);
        }
#endif
        var screenPos = cam.WorldToScreenPoint(worldPos);
        return screenPos;
    }

    public void OnSceneLoaded()
    {
        textBubble = Managers.Instance.UIManager.Show<UITextBubble>();
        textBubble.HideDirect();
    }

    public void OnSceneUnloaded()
    {
        sceneSpeakers.Clear();
        cutSceneSpeakers.Clear();
    }
}