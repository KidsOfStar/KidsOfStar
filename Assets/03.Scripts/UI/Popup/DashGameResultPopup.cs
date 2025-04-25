using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DashGameResultPopup : PopupBase
{
    [Header("Dialogue UI")]
    [SerializeField] private TextMeshProUGUI jigimText;
    [SerializeField] private TextMeshProUGUI semyungText;

    [Header("Dialogue Data")]
    [SerializeField] private DashDialogueData dialogueDatabase;

    private int currentLineIndex = 0;
    private List<string> currentDialogLines;
    private NPCType currentNpcType;

    
    public override void Opened(params object[] param)
    {
        base.Opened(param);

        SkillBTN skillBTN = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel;

        if (param.Length < 2 || !(param[0] is float clearTime) || !(param[1] is NPCType npcType))
        {
            Debug.LogError("Invalid parameters passed to DashGameResultPopup.");
            return; 
        }

        currentNpcType = npcType;
        currentDialogLines = dialogueDatabase.GetDialogueByNpc(clearTime, npcType);

        currentLineIndex = 0;

    }

    public void OnClickDialogue()
    {
        if (currentDialogLines == null) return;

        if (currentLineIndex < currentDialogLines.Count)
        {
            ShowCurrentLine();
        }
        else
        {
            Managers.Instance.UIManager.Hide<DashGameResultPopup>();
        }
    }

    private void ShowCurrentLine()
    {
        if (currentDialogLines == null || currentLineIndex >= currentDialogLines.Count)
        {
            Debug.LogWarning("ShowCurrentLine: 대사 없음 혹은 인덱스 초과");
            return;
        }

        string line = currentDialogLines[currentLineIndex];
        Debug.Log($"[대사 출력] {line}");

        if (currentNpcType == NPCType.Jigim)
        {
            jigimText.text = line;
            semyungText.text = string.Empty;
        }
        else if (currentNpcType == NPCType.Semyung)
        {
            jigimText.text = string.Empty;
            semyungText.text = line;
        }
        currentLineIndex++;
    }
}
