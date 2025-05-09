using System.Linq;
using TMPro;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;

public class DoorPopup : PopupBase
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private TextMeshProUGUI boxWarningText;
    [SerializeField] private TextMeshProUGUI boxFallingText;

    public override void Opened(params object[] param)
    {
        base.Opened(param);

        ShowCurrentWarning();
    }

    private void ShowCurrentWarning()
    {
        // 텍스트 숨겨두고, 해당 타입만 활성화
        messageText.gameObject.SetActive(false);
        boxWarningText.gameObject.SetActive(false);
        boxFallingText.gameObject.SetActive(false);

        // 현재 경고 타입 가져오기
        messageText.text = "잠겨있다. 풀 방법이 없을까?";
        messageText.gameObject.SetActive(true);
    }
}

