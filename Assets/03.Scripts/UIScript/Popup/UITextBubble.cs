using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITextBubble : UIBase
{
    [SerializeField] private RectTransform rectTr;
    [SerializeField] private Image bubbleImage;
    [SerializeField] private RectTransform tailRectTr;
    [SerializeField] private TextMeshProUGUI dialogText;

    private readonly StringBuilder dialogSb = new();
    private readonly WaitForSeconds textWaitTime = new(0.1f);
    private Coroutine dialogCoroutine;
    private bool isTyping = false;

    public void SetDialog(string dialog, Vector3 pos)
    {
        StartDialogCoroutine(dialog);
        rectTr.localPosition = pos;
    }

    private void StartDialogCoroutine(string dialog)
    {
        if (dialogCoroutine != null)
            StopCoroutine(dialogCoroutine);
        
        dialogCoroutine = StartCoroutine(ShowDialog(dialog));
    }
    
    // TODO: 텍스트 설정 생기면 연결
    private IEnumerator ShowDialog(string dialog)
    {
        isTyping = true;
        dialogSb.Clear();
        dialogText.text = string.Empty;
        
        for (int i = 0; i < dialog.Length; i++)
        {
            dialogSb.Append(dialog[i]);
            dialogText.text = dialogSb.ToString();

            if (!isTyping)
            {
                dialogText.text = dialog;
                yield break;
            }
            
            yield return textWaitTime;
        }

        isTyping = false;
    }
    
    private void SkipTyping()
    {
        if (isTyping)
            isTyping = false;
        else
        {
            // 다음 대사 출력
        }
    }

    private void OnEnable()
    {
        Managers.Instance.DialogueManager.OnClick += SkipTyping;
    }

    private void OnDisable()
    {
        Managers.Instance.DialogueManager.OnClick -= SkipTyping;
    }
}
