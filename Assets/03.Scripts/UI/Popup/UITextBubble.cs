using Febucci.UI;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class UITextBubble : UIBase
{
    [Header("Text Bubble")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform rectTr;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private TypewriterByCharacter typewriter;
    [SerializeField] private float clickIgnoreTime = 0.1f;

    private readonly StringBuilder dialogSb = new();
    private readonly WaitForSeconds textWaitTime = new(0.1f);
    private Coroutine dialogCoroutine;
    
    private bool isTyping = false;
    private float dialogStartTime;

    public void InitCamera()
    {
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Managers.Instance.GameManager.MainCamera;
        rectTr.localScale = new Vector3(0.01f, 0.01f, 0.01f);
    }
    
    public void SetDialog(string dialog, Transform bubbleTr)
    {
        StartDialogCoroutine(dialog);
        rectTr.SetParent(bubbleTr);
        rectTr.localPosition = Vector3.zero;
        dialogStartTime = Time.time;
    }

    private void StartDialogCoroutine(string dialog)
    {
        if (dialogCoroutine != null)
        {
            StopCoroutine(dialogCoroutine);
            dialogCoroutine = null;
        }
        
        dialogText.text = string.Empty;
        dialogSb.Clear();
        
        dialogCoroutine = StartCoroutine(ShowDialog(dialog));
    }
    
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
        if (Time.time - dialogStartTime < clickIgnoreTime)
            return;
        
        if (isTyping)
            isTyping = false;
        else
        {
            Managers.Instance.DialogueManager.OnDialogLineComplete();
        }
    }

    public override void HideDirect()
    {
        rectTr.SetParent(null);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        dialogStartTime = 0f;
        Managers.Instance.DialogueManager.OnClick -= SkipTyping;
        Managers.Instance.DialogueManager.OnClick += SkipTyping;
    }

    private void OnDisable()
    {
        Managers.Instance.DialogueManager.OnClick -= SkipTyping;
    }
}
