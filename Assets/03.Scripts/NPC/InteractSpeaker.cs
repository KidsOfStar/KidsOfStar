using System.Collections.Generic;
using UnityEngine;

public abstract class InteractSpeaker : MonoBehaviour
{
    public Dictionary<int, int> DialogByProgress { get; private set; }

    public void InitDialogByProgress()
    {
        var dict = Managers.Instance.DataManager.GetNpcDataDict();
        var startRange = ((int)Managers.Instance.GameManager.CurrentChapter + 1) * 100;
        var endRange = startRange + 99;

        for (int i = 0; i < dict.Count; i++)
        {
            var npcData = dict[i];
            if (npcData == null) continue;
            if (npcData.DialogIndex < startRange || npcData.DialogIndex > endRange) continue;

            DialogByProgress.Add(npcData.Progress, i);
        }
    }
}