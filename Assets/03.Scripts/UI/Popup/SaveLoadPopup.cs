using TMPro;
using UnityEngine.UI;

public class SaveLoadPopup : PopupBase
{
    public TextMeshProUGUI saveTimeText;
    public Button saveButton;
    public Button loadButton;
    public Button deleteButton;

    private int selectedIndex = 0; 
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        
    }

    public void OnSaveButtonClick(int index)
    {
        Managers.Instance.SaveManager.Save(index, (timeStamp) =>
        {
            saveTimeText.text = timeStamp;

        });
    }

    public void OnLoadButtonClick(int index)
    {
        string dummyTimeStamp = "";
        Managers.Instance.SaveManager.Load(index, dummyTimeStamp);
    }
}
