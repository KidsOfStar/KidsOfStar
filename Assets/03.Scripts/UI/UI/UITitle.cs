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
        // 버튼 클릭 이벤트 등록
        startBtn.onClick.AddListener(OnStartBtnClick);
        exitBtn.onClick.AddListener(OnExitBtnClick);
        optionBtn.onClick.AddListener(OnOptionBtnClick);
        loadBtn.onClick.AddListener(OnLoadBtnClick);
    }

    private void OnLoadBtnClick()
    {
        // 로드 Popup 띄우기
        //Managers.Instance.UIManager.Show<LoadPopup>();
        Managers.Instance.UIManager.Show<SavePopup>();

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
        NextScene();
    }

    private void NextScene()
    {
        // 다음 씬으로 이동
        Managers.Instance.SceneLoadManager.LoadScene(SceneType.Chapter01);
        //Managers.Instance.cutSceneManager.MapSceneToCutScene("Chapter01", "Intro");
    }
}
