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
/// UI의 기본 클래스
/// </summary>
[System.Serializable]
public class UIOptions
{
    public bool isDestroyOnHide = false; // 꺼질 때 파괴 여부
}
public abstract class UIBase : MonoBehaviour
{
    [Header("UI 위치 설정")]
    public eUIPosition uiPosition;

    [Header("UI 옵션")]
    public UIOptions uiOptions = new UIOptions();

    public event Action<object[]> closed; // closed 이벤트 추가

    public abstract void Opened(params object[] param);
    public abstract void HideDirect();
    public abstract void SetActive(bool isActive);
}
