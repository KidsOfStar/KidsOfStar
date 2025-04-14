using MainTable;
using System.Collections.Generic;

public class DataManager
{
    private readonly Dictionary<int, DialogData> dialogDatas;
    
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