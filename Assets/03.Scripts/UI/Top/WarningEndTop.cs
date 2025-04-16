using UnityEngine.UI;
using UnityEngine;

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
#elif UNITY_ANDROID
        // 게임 종료
        Application.Quit(); // 빌드된 게임에서 종료
#elif UNITY_WEBGL
        ShowExitPopup("웹에서는 게임을 종료할 수 없습니다.\n브라우저 탭을 닫아주세요.");
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

    void ShowExitPopup(string message)
    {
        // 종료 팝업 UI 띄우기 (예: 팝업 텍스트 설정, UI 활성화 등)
        Debug.Log(message);
    }
}
