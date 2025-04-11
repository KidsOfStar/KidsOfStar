using MainTable;
using System.Collections.Generic;

public class DataManager
{
    private Dictionary<int, DialogData> dialogDatas = new();
    
    public DataManager()
    {
        dialogDatas = DialogData.GetDictionary();
    }

    public DialogData GetPlayerData(int index)
    {
        if (dialogDatas.TryGetValue(index, out var data))
            return data;
            
        else
        {
            EditorLog.LogError($"DataManager : Not found PlayerData with index: {index}");
            return null;
        }
    }
}