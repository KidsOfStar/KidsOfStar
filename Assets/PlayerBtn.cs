using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 플레이어 관련 UI를 제어하는 클래스
public class PlayerBtn : UIBase
{
    [Header("UI FunctionBtn")] // 게임 기능 버튼들
    public Button stopBtn;
    public Button skipBtn;

    [Header("UI Panel")] // 패널
    public GameObject skillPanel; // 스킬 패널
    public GameObject functionPanel; // 기능 패널


    private void Start()
    {
        stopBtn.onClick.AddListener(OnOptionBtnClick);
        skipBtn.onClick.AddListener(OnSkip);
    }
    
    // 정지 버튼 클릭 시 게임 일시정지
    private void OnOptionBtnClick()
    {
        Managers.Instance.UIManager.Show<OptionPopup>();
        Time.timeScale = 0;
    }

    
    // 스킵 버튼 클릭 시 호출될 메소드 (현재 비어 있음)
    public void OnSkip()
    {
        // 씬 스킵 처리
    }

    public void CutSceneSkip()
    {
        skillPanel.SetActive(false); // 스킬 패널 비활성화
        functionPanel.SetActive(false); // 기능 패널 비활성화
        stopBtn.gameObject.SetActive(true); // 정지 버튼 비활성화
    }
}
