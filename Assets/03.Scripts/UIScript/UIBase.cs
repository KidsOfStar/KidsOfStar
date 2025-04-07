using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public enum eUIPosition
{
    Top,
    Bottom,
    Left,
    Right,
    Center,
    UI,
}

/// <summary>
/// UI�� �⺻ Ŭ����
/// </summary>
[System.Serializable]
public class UIOptions
{
    public bool isDestroyOnHide = false; // ���� �� �ı� ����
}
public abstract class UIBase : MonoBehaviour
{
    [Header("UI ��ġ ����")]
    public eUIPosition uiPosition;

    [Header("UI �ɼ�")]
    public UIOptions uiOptions = new UIOptions();

    public event Action<object[]> closed; // closed �̺�Ʈ �߰�

    public abstract void Opened(params object[] param);
    public abstract void HideDirect();
    public abstract void SetActive(bool isActive);
}
