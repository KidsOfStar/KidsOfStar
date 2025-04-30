using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UITitle : UIBase
{
    [Header("Button")]
    public Button startBtn;
    public Button exitBtn;
    public Button optionBtn;
    public Button loadBtn;

    
    // Start is called before the first frame update
    void Start()
    {
        // 버튼 클릭 이벤트 등록
        //startBtn.onClick.AddListener(OnStartBtnClick);
        //exitBtn.onClick.AddListener(OnExitBtnClick);
        //optionBtn.onClick.AddListener(OnOptionBtnClick);
        //loadBtn.onClick.AddListener(OnLoadBtnClick);

        OnClickListener(startBtn, OnStartBtnClick, SfxSoundType.UIButton);
        OnClickListener(exitBtn, OnExitBtnClick, SfxSoundType.UIButton);
        OnClickListener(optionBtn, OnOptionBtnClick, SfxSoundType.UIButton);
        OnClickListener(loadBtn, OnLoadBtnClick, SfxSoundType.UIButton);
    }

    private void OnClickListener(Button button, UnityAction callback, SfxSoundType sfxType)
    {
        button.onClick.AddListener(() =>
        {
            Managers.Instance.SoundManager.PlaySfx(sfxType); // 효과음 재생
            callback?.Invoke(); // 콜백 실행
        });
    }

    private void OnLoadBtnClick()
    {
        // 로드 Popup 띄우기
        Managers.Instance.UIManager.Show<LoadPopup>();
        //Managers.Instance.UIManager.Show<SavePopup>();

    }

    private void OnOptionBtnClick()
    {
        // 옵션 Popup 띄우기
        Managers.Instance.UIManager.Show<OptionPopup>();
    }

    private void OnExitBtnClick()
    {
        Managers.Instance.UIManager.Show<WarningEndTop>(); // 종료 경고창 띄우기
    }

    private void OnStartBtnClick()
    {
        // 데이터가 있을 경우 
        //Managers.Instance.UIManager.Show<WarningStartTop>();

        // 아닐 경우 게임 시작
        //NextScene();

        StartCoroutine(LoadSceneCoroutine("Chapter01"));
    }

    private void NextScene()
    {
        // 다음 씬으로 이동
        Managers.Instance.SceneLoadManager.LoadScene(SceneType.Chapter1);
        //Managers.Instance.cutSceneManager.MapSceneToCutScene("Chapter01", "Intro");
    }

    IEnumerator LoadSceneCoroutine(string sceneName)
    {
        // 씬 로드 코루틴
        yield return new WaitUntil(() => Managers.Instance.SceneLoadManager.IsSceneLoadComplete);
        Managers.Instance.SceneLoadManager.LoadScene(SceneType.Chapter1);
    }
}
