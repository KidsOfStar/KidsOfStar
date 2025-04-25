using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ENpcType
{
    None,
    Jigim,
    Semyung
}

public class DashGameResultPopup : PopupBase
{
    [Header("Dialogue UI")]
    [SerializeField] private TextMeshProUGUI jigimText;
    [SerializeField] private TextMeshProUGUI semyungText;

    [Header("Dialogue Data")]
    [SerializeField] private DashDialogueDatabase dialogueDatabase;

    private int currentLineIndex = 0;
    private List<string> currentDialogLines;
    private ENpcType currentNpcType;

    
    public override void Opened(params object[] param)
    {
        base.Opened(param);

        SkillBTN skillBTN = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel;

        if (param.Length < 2 || !(param[0] is float clearTime) || !(param[1] is ENpcType npcType))
        {
            Debug.LogError("Invalid parameters passed to DashGameResultPopup.");
            return; 
        }

        Debug.Log($"[Opened] 시간: {clearTime}, NPC: {npcType}");

        currentNpcType = npcType;
        currentDialogLines = dialogueDatabase.GetDialogueByNpc(clearTime, npcType);
        Debug.Log($"[대사 수] {currentDialogLines?.Count}");

        currentLineIndex = 0;
        ShowCurrentLine();
    }

    public void OnClickDialogue()
    {
        if (currentDialogLines == null) return;

        currentLineIndex++;

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

        if (currentNpcType == ENpcType.Jigim)
        {
            jigimText.text = line;
            semyungText.text = string.Empty;
        }
        else if (currentNpcType == ENpcType.Semyung)
        {
            jigimText.text = string.Empty;
            semyungText.text = line;
        }
    }
}
