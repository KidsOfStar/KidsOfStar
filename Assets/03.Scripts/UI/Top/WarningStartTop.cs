using UnityEngine.UI;

public class WarningStartTop : TopBase
{
    public Button closeBtn;
    public Button appltBtn;


    void Start()
    {
        closeBtn.onClick.AddListener(OnClickClose);
        appltBtn.onClick.AddListener(NextSence);
    }

    //public override void Opened(params object[] param)
    //{
    //    ShowTopTitle("경고창");
    //}

    public override void HideDirect()
    {
        gameObject.SetActive(false);
    }

    public void OnClickClose()
    {
        HideDirect();
    }

    public void NextSence()
    {
        // 다음 단계로 진행하는 로직
    }
}
