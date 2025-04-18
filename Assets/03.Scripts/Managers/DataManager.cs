using MainTable;
using System.Collections.Generic;

public class DataManager
{
    private readonly Dictionary<int, DialogData> dialogDatas;
    private readonly Dictionary<int, NPCData> npcDatas;
    
    public DataManager()
    {
        dialogDatas = DialogData.GetDictionary();
        npcDatas = NPCData.GetDictionary();
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
    
    public Dictionary<int, NPCData> GetNpcDataDict()
    {
        return npcDatas;
    }
    
    public NPCData GetNpcData(int index)
    {
        if (npcDatas.TryGetValue(index, out var data))
            return data;
            
        else
        {
            EditorLog.LogError($"DataManager : Not found NPCData with index: {index}");
            return null;
        }
    }
}