using UnityEngine;
using UnityEngine.UI;

public class PopupBase : UIBase
{
    [SerializeField] protected Button closeBtn;
    [Tooltip("닫을 때 Time.timeScale = 1로 복구할지 여부")]
    [SerializeField] protected bool resumeTimeOnClose = false;

    protected virtual void Start()
    {
        if(closeBtn != null)
        {
            closeBtn.onClick.AddListener(OnClose);
        }
    }
    public virtual void OnClose()
    {
        EditorLog.Log($"timeScale + {resumeTimeOnClose}");
        if (resumeTimeOnClose && Time.timeScale == 0)
        {
            Time.timeScale = 1; // 게임 재개
        }
        HideDirect();
    }

}
