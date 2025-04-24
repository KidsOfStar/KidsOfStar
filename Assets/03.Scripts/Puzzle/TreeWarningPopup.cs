using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TreeWarningPopup : PopupBase
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private TextMeshProUGUI boxWarningText;
    [SerializeField] private Button confirmButton;

    // WarningType의 값을 순서대로 저장
    private WarningType[] warningQueue;
    // 현재 화면에 표시되고 있는 warningQueue내의 인덱스
    private int queueIndex;

    public override void Opened(params object[] param)
    {
        base.Opened(param);

        // param 매개변수를 통해 warningType의 갯수를 count에 저장
        int count = 0;
        for (int i = 0; i < param.Length; i++)
        {
            if (param[i] is WarningType)
                count++;
        }

        warningQueue = new WarningType[count];

        int nextIndex = 0;
        for (int i = 0; i < param.Length; i++)
        {
            if (param[i] is WarningType warningTxt)
            {
                warningQueue[nextIndex++] = warningTxt;
            }
        }

        queueIndex = 0;

        // 버튼 리스너 초기화 및 등록
        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(OnConfirmPressed);

        // 첫 경고 표시
        ShowCurrentWarning();
    }

         private void ShowCurrentWarning()
    {
        // 현재 경고 타입 가져오기
        var type = warningQueue[queueIndex];

        // 텍스트 숨겨두고, 해당 타입만 활성화
        messageText.gameObject.SetActive(false);
        boxWarningText.gameObject.SetActive(false);

        switch (type)
        {
            case WarningType.Squirrel:
                messageText.text = "세심하게 만져야 한다. 다른 방법이 없을까?";
                messageText.gameObject.SetActive(true);
                break;

            case WarningType.BoxMissing:
                boxWarningText.text = "중요한 상자를 두고왔다. 얼른 가서 다시 찾아오자";
                boxWarningText.gameObject.SetActive(true);
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

