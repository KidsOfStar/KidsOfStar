using TMPro;
using UnityEngine;

public class LoadSlot
{
    public TextMeshProUGUI loadSlootName;
    public TextMeshProUGUI loadSlootData;


    public void UpdateUI()
    {

    }
    public void SetData()
    {
        // 저장된 슬롯 이름과 데이터
        loadSlootName.text = "Save Slot 1";
        loadSlootData.text = "Chapter1 25-03-224 14.14.38";
    }

    public void OnClick()
    {

    }

    public class SaveData
    {

    }
}
