using UnityEngine.UI;

public class SaveLoadPopup : UIBase
{
    public Button closeBtn;

    // Start is called before the first frame update
    void Start()
    {
        closeBtn.onClick.AddListener(OnClickClosebtn);
    }

    public void OnClickClosebtn()
    {
        HideDirect();
    }
}
