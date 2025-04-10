using UnityEngine.UI;

public class WarningEndTop : TopBase
{
    public Button closeBtn;
    public Button appltBtn;

    // Start is called before the first frame update
    void Start()
    {
        appltBtn.onClick.AddListener(OnExitBtnClick);
        closeBtn.onClick.AddListener(OnClickClose);
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
    public override void HideDirect()
    {
        gameObject.SetActive(false);
    }
    public void OnClickClose()
    {
        HideDirect();
    }
}
