using System;
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

[Serializable]
public class UIOptions
{
    public bool isDestroyOnHide = false;
}

public abstract class UIBase : MonoBehaviour
{
    [Header("UI ��ġ ����")]
    public eUIPosition uiPosition = eUIPosition.UI;

    [Header("UI �ɼ� ����")]
    public UIOptions uiOptions = new UIOptions();

    // ���� �� ȣ��� �ݹ�
    public Action<object[]> closed;

    /// <summary>
    /// UI�� ���� �� ȣ��Ǵ� �Լ�
    /// </summary>
    /// <param name="param"></param>
    public virtual void Opened(params object[] param) { }

    /// <summary>
    /// UI�� ��� ��Ȱ��ȭ�� �� ȣ��Ǵ� �Լ�
    /// </summary>
    public virtual void HideDirect()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// UI Ȱ��ȭ/��Ȱ��ȭ
    /// </summary>
    /// <param name="isActive"></param>
    public virtual void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
