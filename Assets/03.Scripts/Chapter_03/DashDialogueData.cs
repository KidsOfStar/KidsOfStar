using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueEntry
{
    public float timeThreshold; // 기준 시간 (초 단위: 90, 150, 210)
    public List<string> jigimDialogues;   // 지김 대사 (문단별)
    public List<string> semyungDialogues; // 세명 대사 (문단별)
}

// ScriptableObject로 여러 시간 구간의 대사를 포함
[CreateAssetMenu(fileName = "DialogueData", menuName = "ScriptableObject/DialogueData")]
public class DashDialogueData : ScriptableObject
{
    public List<DialogueEntry> entries;

    public List<string> GetDialogueByNpc(float clearTime, NPCType npcType)
    {
        DialogueEntry selected = GetDialogueEntry(clearTime);
        return npcType switch
        {
            NPCType.Jigim => selected.jigimDialogues,
            NPCType.Semyung => selected.semyungDialogues,
            _ => new List<string>()
        };
    }

    private DialogueEntry GetDialogueEntry(float time)
    {
        foreach (var entry in entries)
        {
            if (time < entry.timeThreshold)
                return entry;
        }
        return entries[entries.Count - 1]; // 가장 마지막 구간 반환
    }
}