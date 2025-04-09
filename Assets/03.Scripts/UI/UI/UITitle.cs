using System;
using UnityEngine;
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
        Opened();
    }

    public override void Opened(params object[] param)
    {
        // 버튼 클릭 이벤트 등록
        startBtn.onClick.AddListener(OnStartBtnClick);
        exitBtn.onClick.AddListener(OnExitBtnClick);
        optionBtn.onClick.AddListener(OnOptionBtnClick);
        loadBtn.onClick.AddListener(OnLoadBtnClick);
    }

    private void OnLoadBtnClick()
    {
        // 로드 Popup 띄우기
         Managers.Instance.UIManager.Show<SaveLoadPopup>();
    }

    private void OnOptionBtnClick()
    {
        // 옵션 Popup 띄우기
        Managers.Instance.UIManager.Show<OptionPopup>();
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

    private void OnStartBtnClick()
    {
        // 나중에 데이터가 있을 경우 
        Managers.Instance.UIManager.Show<WarningTop>();

        // 게임 시작
        NextScene();
        EditorLog.Log("게임 시작"); // 게임 시작 로그
    }

    private void NextScene()
    {
        // 다음 씬으로 이동
    }
}
