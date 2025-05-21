using TMPro;
using UnityEngine;

public class DoorPopup : PopupBase
{
    [SerializeField] private TextMeshProUGUI messageText;

    public override void Opened(params object[] param)
    {
        base.Opened(param);

        ShowCurrentWarning();
    }

    private void ShowCurrentWarning()
    {
        // 텍스트 숨겨두고, 해당 타입만 활성화
        messageText.gameObject.SetActive(true);


        // 현재 경고 타입 가져오기
        messageText.text = "잠겨있다. 풀 방법이 없을까?";
    }

    public void SetText(string text)
    {
        // 텍스트 숨겨두고, 해당 타입만 활성화
        messageText.gameObject.SetActive(true);

        // 현재 경고 타입 가져오기
        messageText.text = text;
    }
}

