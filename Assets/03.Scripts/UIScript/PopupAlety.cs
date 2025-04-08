using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupAlety : UIBase
{

    public TextMeshProUGUI titleText;   // ���� �ؽ�Ʈ
    public TextMeshProUGUI descText; // ���� �ؽ�Ʈ

    public Button closeButton; // �ݱ� ��ư

    void Awake()
    {
        closeButton.onClick.AddListener(OnClickClose); // �ݱ� ��ư Ŭ�� �� OnClickClose �Լ� ȣ��
    }

    public override void Opened(params object[] param)
    {
        titleText.text = (string)param[0]; // ù ��° �Ķ���͸� �������� ����
        descText.text = (string)param[1]; // �� ��° �Ķ���͸� �������� ����
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
