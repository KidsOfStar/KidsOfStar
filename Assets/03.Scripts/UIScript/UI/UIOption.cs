using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIOption : UIBase
{
    [Header("UI Audio")]
    public Slider bgmSlider; // BGM 슬라이더
    public Slider sfxSlider; // SFX 슬라이더


    [Header("UI Btn")]
    public Button closeBtn; // 닫기 버튼
    public Button retryBtn; // 재시작 버튼
    public Button endBtn; // 메인 메뉴 버튼

    private SoundManager soundManager;
    void Start()
    {
        soundManager = Managers.Instance.SoundManager;

        InitSlider();
        ButtonClick();
    }

    private void InitSlider()
    {
       
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 0.7f); // 기본값 0.7
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.8f); // 기본값 0.8

        //Managers.Instance.GameManager.SetBgmVolume(bgmSlider.value);
        //Managers.Instance.GameManager.SetSfxVolume(sfxSlider.value);

        //// 슬라이더 초기화
        bgmSlider.onValueChanged.AddListener(soundManager.SetBgmVolume);
        sfxSlider.onValueChanged.AddListener(soundManager.SetSfxVolume);
    }


    public void ButtonClick()
    {
        // 버튼 클릭 이벤트 등록
        closeBtn.onClick.AddListener(OnClickCloseBtn);
        retryBtn.onClick.AddListener(OnClickRetryBtn);
        endBtn.onClick.AddListener(OnExitBtnClick);
    }

    public void OnClickCloseBtn()
    {
        HideDirect();
    }

    public void OnClickRetryBtn()
    {
        // 현재 씬에서 재시작
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    private void OnExitBtnClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터에서 실행 중지
#else
        // 게임 종료
        Application.Quit(); // 빌드된 게임에서 종료
#endif
    }

}
