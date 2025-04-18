using MainTable;
using System.Collections.Generic;

public class DataManager
{
    private readonly Dictionary<int, DialogData> dialogDatas;
    private readonly Dictionary<int, NPCData> npcDatas;
    private readonly Dictionary<ChapterType, int> maxProgressDict = new();
    
    public DataManager()
    {
        dialogDatas = DialogData.GetDictionary();
        npcDatas = NPCData.GetDictionary();
        LoadChapterProgressData();
    }

    public void LoadChapterProgressData()
    {
        var data = Managers.Instance.ResourceManager.Load<ChapterData>(Define.DataPath + "ChapterData", true);
        if (data == null)
        {
            EditorLog.LogError("DataManager : ChapterData is null");
            return;
        }

        for (int i = 0; i < data.chapterProgresses.Length; i++)
        {
            var chapterProgress = data.chapterProgresses[i];
            if (chapterProgress != null)
            {
                maxProgressDict[chapterProgress.chapterType] = chapterProgress.maxProgress;
            }
        }
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
    
    public int GetMaxProgress(ChapterType chapterType)
    {
        if (maxProgressDict.TryGetValue(chapterType, out var maxProgress))
            return maxProgress;
            
        else
        {
            EditorLog.LogError($"DataManager : Not found MaxProgress with chapterType: {chapterType}");
            return 0;
        }
    }
}