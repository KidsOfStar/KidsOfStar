using System.Linq;
using TMPro;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;

public class TreeWarningPopup : PopupBase
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private TextMeshProUGUI boxWarningText;
    [SerializeField] private TextMeshProUGUI boxFallingText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button retryButton;

    // WarningType의 값을 순서대로 저장
    private WarningType[] warningQueue;

    // 현재 화면에 표시되고 있는 warningQueue내의 인덱스
    private int queueIndex;

    private ObjectIndicator objIndicator;

    public override void Opened(params object[] param)
    {
        base.Opened(param);

        warningQueue = param.OfType<WarningType>().ToArray();
        queueIndex = 0;

        objIndicator = FindObjectOfType<ObjectIndicator>();
        //TODO: FindObjectOfType<>()수정

        // 첫 경고 표시
        ShowCurrentWarning();
    }

    private void ShowCurrentWarning()
    {
        // 텍스트 숨겨두고, 해당 타입만 활성화
        messageText.gameObject.SetActive(false);
        boxWarningText.gameObject.SetActive(false);
        boxFallingText.gameObject.SetActive(false);
        confirmButton.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);

        confirmButton.onClick.RemoveAllListeners();
        retryButton.onClick.RemoveAllListeners();   

        // 현재 경고 타입 가져오기
        switch (warningQueue[queueIndex])
        {
            case WarningType.Squirrel:
                messageText.text = "세심하게 만져야 한다. 다른 방법이 없을까?";
                messageText.gameObject.SetActive(true);
                ButtonWithSfx(confirmButton, SfxSoundType.ButtonPush, OnConfirmPressed);
                break;

            case WarningType.BoxMissing:
                boxWarningText.text = "박스를 문 앞으로 가져와야할 것 같다."; 
                boxWarningText.gameObject.SetActive(true);
                ButtonWithSfx(confirmButton, SfxSoundType.ButtonPush, OnConfirmPressed);
                break;

            case WarningType.BoxFalling:
                boxFallingText.text = "상자가 떨어졌다. 다시 시도해보자.";
                boxFallingText.gameObject.SetActive(true);
                ButtonWithSfx(confirmButton, SfxSoundType.ButtonPush, objIndicator.ResetPosition);
                break;

            default:
                messageText.text = "…";
                boxWarningText.text = "…";
                messageText.gameObject.SetActive(true);
                boxWarningText.gameObject.SetActive(true);
                break;
        }
    }

    private void OnConfirmPressed()
    {
        queueIndex++;

        // 다음 경고 표시
        if (queueIndex < warningQueue.Length)
            ShowCurrentWarning();

        // 모두 다 봤으면 팝업 닫기
        else
            Managers.Instance.UIManager.Hide<TreeWarningPopup>();
    }

    private void ButtonWithSfx(Button btn, SfxSoundType sfx, UnityAction action)
    {
        btn.gameObject.SetActive(true);
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() =>
        {
            Managers.Instance.SoundManager.PlaySfx(sfx);
            action();
        });
    }
}

