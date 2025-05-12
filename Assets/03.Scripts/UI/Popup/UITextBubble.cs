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
    [SerializeField] private TextMeshProUGUI dialogTmp;
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
        ShowDialogue(dialog);
    }
    
    // 한글자 출력할 때마다 발생하는 이벤트에 isTyping이 false라면 전부 다 출력하는 함수를 등록
    // 모든 글자가 출력됐을 때 발생하는 이벤트에 isTyping = false; 를 등록
    // 출력 끝났을 때 이벤트 해제

    private void ShowDialogue(string dialog)
    {
        isTyping = true;
        dialogTmp.text = string.Empty;
        
        typewriter.onCharacterVisible.AddListener(_ => CheckSkipTyping());
        typewriter.onTextShowed.AddListener(() => isTyping = false);
        typewriter.ShowText(dialog);
    }

    private void CheckSkipTyping()
    {
        if (!isTyping)
            typewriter.SkipTypewriter();
    }
    
    private void SkipTyping()
    {
        if (Time.time - dialogStartTime < clickIgnoreTime)
            return;
        
        if (isTyping)
            isTyping = false;
        else
        {
            typewriter.onCharacterVisible.RemoveAllListeners();
            typewriter.onTextShowed.RemoveAllListeners();
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
