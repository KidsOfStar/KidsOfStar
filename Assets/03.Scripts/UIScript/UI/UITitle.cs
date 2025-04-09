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
        // UIManager.Instance.ShowPopupUI<UILoad>(); (?)
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
        // ScemeLoadManager.Instance.LoadScene(SceneType.Game);
        EditorLog.Log("게임 시작"); // 게임 시작 로그
    }
}
