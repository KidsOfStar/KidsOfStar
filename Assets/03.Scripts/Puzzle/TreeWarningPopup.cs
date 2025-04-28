using System.Linq;
using TMPro;
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

        // 첫 경고 표시
        ShowCurrentWarning();
    }

    private void ShowCurrentWarning()
    {
        // 텍스트 숨겨두고, 해당 타입만 활성화
        messageText.gameObject.SetActive(false);
        boxWarningText.gameObject.SetActive(false);
        boxFallingText.gameObject.SetActive(false);

        confirmButton.onClick.RemoveAllListeners();
        retryButton.onClick.RemoveAllListeners();
        confirmButton.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);

        // 현재 경고 타입 가져오기
        switch (warningQueue[queueIndex])
        {
            case WarningType.Squirrel:
                messageText.text = "세심하게 만져야 한다. 다른 방법이 없을까?";
                messageText.gameObject.SetActive(true);
                confirmButton.gameObject.SetActive(true);
                confirmButton.onClick.AddListener(OnConfirmPressed);
                break;

            case WarningType.BoxMissing:
                boxWarningText.text = "중요한 상자를 두고왔다. 얼른 가서 다시 찾아오자";
                boxWarningText.gameObject.SetActive(true);
                confirmButton.gameObject.SetActive(true);
                confirmButton.onClick.AddListener(OnConfirmPressed);
                break;

            case WarningType.BoxFalling:
                boxFallingText.text = "중요한 상자를 떨어뜨렸다. 다시 게임을 플레이해보자!";
                boxFallingText.gameObject.SetActive(true);
                retryButton.gameObject.SetActive(true);
                retryButton.onClick.AddListener(objIndicator.ResetPosition);
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

        if (queueIndex < warningQueue.Length)
        {
            // 다음 경고 표시
            ShowCurrentWarning();
        }
        else
        {
            // 모두 다 봤으면 팝업 닫기
            Managers.Instance.UIManager.Hide<TreeWarningPopup>();
        }
    }
}

