using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupBase : UIBase
{
    [SerializeField] protected Button closeBtn;
    [Tooltip("닫을 때 Time.timeScale = 1로 복구할지 여부")]
    public bool checkTimeStop = false;
    public bool isFirst = false;

    private List<GameObject> uiHide;   // 숨길 UI

    public override void Opened(params object[] param)
    {
        if(checkTimeStop) 
            Time.timeScale = 0; // 게임 일시 정지
        // 특정 bool를 이용해서 앞에서 보여지게 하기
        if(isFirst)
            transform.SetAsLastSibling();

        //HideUI(false); // UI 비활성화
        
    }

    protected virtual void Start()
    {
        Managers.Instance.GameManager.Player.Controller.LockPlayer();// 플레이어 잠금

        if (closeBtn != null)
        {
            closeBtn.onClick.AddListener(() => 
            {
                Managers.Instance.SoundManager.PlaySfx(SfxSoundType.UICancel);
                HideDirect();
                //HideUI(true); // UI 활성화
                Managers.Instance.GameManager.Player.Controller.UnlockPlayer(); 
                Debug.Log("PopupBase Close");
            });
        }

        //// 팝업이 열릴 때 UI 숨기기
        //uiHide = new List<GameObject>()
        //{
        //    Managers.Instance.UIManager.Get<PlayerBtn>().gameObject,
        //    Managers.Instance.UIManager.Get<UIJoystick>().gameObject,
        //};

    }

    public override void HideDirect()
    {
        base.HideDirect();
        if (checkTimeStop)
        {
            Time.timeScale = 1; // 게임 재개
            if (Managers.Instance.SceneLoadManager.CurrentScene != SceneType.Title)
            {
                Managers.Instance.GameManager.Player.Controller.UnlockPlayer();
            }
        }

    }

    //public void HideUI(bool hideUI)
    //{
    //    foreach (var ui in uiHide)
    //    {
    //        ui.SetActive(hideUI); // UI 비활성화
    //    }
    //}

    // 퍼즐이 열리면 플레이어 잠금
    //protected virtual void LockPlayer(bool lockPlayer)
    //{
    //    if (lockPlayer)
    //    {
    //        Managers.Instance.GameManager.Player.Controller.LockPlayer();
    //    }
    //    else
    //    {
    //        Managers.Instance.GameManager.Player.Controller.UnlockPlayer();
    //    }
    //}
}
