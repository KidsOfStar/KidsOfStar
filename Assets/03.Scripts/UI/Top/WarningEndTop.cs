using UnityEngine.UI;

public class WarningEndTop : TopBase
{
    public Button appltBtn;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        appltBtn.onClick.AddListener(OnExitBtnClick);
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
