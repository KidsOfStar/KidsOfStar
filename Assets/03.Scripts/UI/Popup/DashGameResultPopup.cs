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
    private CharacterType currentNpcType;

    
    public override void Opened(params object[] param)
    {
        base.Opened(param);
        SkillBTN skillBTN = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel;

        DisableAllTextBubbles();

        if (param.Length < 2 || !(param[0] is float index) || !(param[1] is CharacterType npcType))
        {
            return; 
        }

        currentNpcType = npcType;
        currentDialogLines = dialogueDatabase.GetDialogueByNpc(index, npcType);


        if (currentDialogLines == null || currentDialogLines.Count == 0)
        {
            Debug.LogWarning($"No dialogues found for NPC: {npcType}, Index: {index}");
        }
        else
        {
            Debug.Log($"Loaded {currentDialogLines.Count} dialogues for NPC: {npcType}, Index: {index}");
            for (int i = 0; i < currentDialogLines.Count; i++)
            {
                Debug.Log($"Dialogue {i + 1}: {currentDialogLines[i]}");
            }
        }

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
            EditorLog.Log("대사 끝");
            Managers.Instance.UIManager.Hide<DashGameResultPopup>();

            Managers.Instance.CutSceneManager.PlayCutScene(CutSceneType.FieldNormalLife.GetName()); // 컷씬 재생
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

        if (currentNpcType == CharacterType.Jigim)
        {
            jigimText.text = line;
            semyungText.text = string.Empty;
        }
        else if (currentNpcType == CharacterType.Semyung)
        {
            jigimText.text = string.Empty;
            semyungText.text = line;
        }
        currentLineIndex++;
    }

    private void DisableAllTextBubbles()
    {
        UITextBubble[] bubbles = FindObjectsOfType<UITextBubble>(true); // (true) 비활성화 된 것도 찾기

        foreach (var bubble in bubbles)
        {
            bubble.gameObject.SetActive(false); // 모든 버블 끄기
        }
    }
}
