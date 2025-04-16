using UnityEngine;
using UnityEngine.UI;

public class TopBase : UIBase
{
    [SerializeField] protected Button closeBtn;
    protected virtual void Start()
    {
        if (closeBtn != null)
        {
            closeBtn.onClick.AddListener(OnClose);
        }
    }
    // Top UI 전용 공통 처리
    public override void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public virtual void OnClose()
    {
        HideDirect();
    }

    public virtual void OnCheck() { }

    //public void ShowTopTitle(string title)
    //{
    //    Debug.Log($"Top UI Title: {title}");
    //    // 여기에 공통 타이틀 처리 가능
    //}
}
