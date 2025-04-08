using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    // Start is called before the first frame update

    private void Awake()
    {
        soundManager = Managers.Instance.SoundManager;
        soundManager.Init();
    }
    void Start()
    {

        ButtonClick();

    }

    private void InitSlider()
    {

    }

    public void ButtonClick()
    {
        // 버튼 클릭 이벤트 등록
        closeBtn.onClick.AddListener(OnClickCloseBtn);
        retryBtn.onClick.AddListener(OnClickRetryBtn);
        endBtn.onClick.AddListener(OnClickEndBtn);
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

    public void OnClickEndBtn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터에서 실행 중지
#else
        // 게임 종료
        Application.Quit(); // 빌드된 게임에서 종료
#endif
    }
}
