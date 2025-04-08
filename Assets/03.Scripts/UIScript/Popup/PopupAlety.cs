using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupAlety : UIBase
{

    public TextMeshProUGUI titleText;   // 제목 텍스트
    public TextMeshProUGUI descText; // 내용 텍스트

    public Button closeButton; // 닫기 버튼

    void Awake()
    {
        closeButton.onClick.AddListener(OnClickClose); // 닫기 버튼 클릭 시 OnClickClose 함수 호출
    }

    public override void Opened(params object[] param)
    {
        titleText.text = (string)param[0]; // 첫 번째 파라미터를 제목으로 설정
        descText.text = (string)param[1]; // 두 번째 파라미터를 내용으로 설정
    }

    public override void HideDirect()
    {
        Managers.Instance.UIManager.Hide<PopupAlety>();
    }

    public void OnClickClose()
    {
        HideDirect();
    }

}
