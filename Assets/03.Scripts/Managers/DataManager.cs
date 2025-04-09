using MainTable;
using System.Collections.Generic;

public class DataManager
{
    // TODO: PlayerData -> DialogData
    private Dictionary<int, PlayerData> playerData = new();
    
    public DataManager()
    {
        playerData = PlayerData.GetDictionary();
    }

    public PlayerData GetPlayerData(int index)
    {
        if (playerData.TryGetValue(index, out var data))
            return data;
            
        else
        {
            EditorLog.LogError($"DataManager : Not found PlayerData with index: {index}");
            return null;
        }
    }
}