using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ENpcType
{
    None,
    Jigim,
    Semyung
}

[System.Serializable]
public class DialogueGroup
{
    public float TimeThreshold; // 기준 시간 (예: 90f, 150f, 210f)
    public List<string> dialogJigim; // NPC 1의 대사 리스트
    public List<string> dialogSemyung; // NPC 2의 대사 리스트
}

public class DashGameResultPopup : PopupBase
{
    [Header("Dialogue UI")]
    [SerializeField] private TextMeshProUGUI jigimText;
    [SerializeField] private TextMeshProUGUI semyungText;

    [Header("Dialogue Data")]
    [SerializeField] private List<DialogueGroup> dialogueGroups;

    private DialogueGroup currentGroup;

    public override void Opened(params object[] param)
    {
        base.Opened(param);

        if (param.Length < 2 || !(param[0] is float clearTime) || !(param[1] is ENpcType npcType)) return;

        currentGroup = GetDialogueGroupByTime(clearTime);

        // NPC 타입에 따라 해당 텍스트만 출력
        switch (npcType)
        {
            case ENpcType.Jigim:
                jigimText.text = string.Join("\n", currentGroup.dialogJigim);
                semyungText.text = string.Empty; // 비우기
                break;
            case ENpcType.Semyung:
                jigimText.text = string.Empty;
                semyungText.text = string.Join("\n", currentGroup.dialogSemyung);
                break;
        }
    }


    private DialogueGroup GetDialogueGroupByTime(float time)
    {
        foreach (var group in dialogueGroups)
        {
            if (time < group.TimeThreshold)
                return group;
        }
        return dialogueGroups[dialogueGroups.Count - 1];
    }
}


