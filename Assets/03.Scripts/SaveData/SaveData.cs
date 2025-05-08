using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class SaveData
{
    // TODO: 현재 폼 저장
    // TODO: enumFlags로 폼 해금 저장

    public string saveName;
    public int difficulty;
    public int chapter;
    public int chapterProgress;
    public Vector3 playerPosition;
    public int[] chapterTrust;
    public bool[] endingDatas;
    public PlayerFormType unlockedPlayerForms;
    // public PlayerFormType currentPlayerForm;

    public void InitData()
    {
        var gameManager = Managers.Instance.GameManager;
        difficulty = (int)gameManager.Difficulty;
        chapter = (int)gameManager.CurrentChapter;
        chapterProgress = gameManager.ChapterProgress;
        playerPosition = gameManager.PlayerPosition;
        unlockedPlayerForms = gameManager.UnlockedForms;
        // currentPlayerForm = gameManager.CurrentForm;
        chapterTrust = gameManager.GetTrustArray();
        endingDatas = gameManager.GetEndingArray();
    }

    public void LoadData()
    {
        Managers.Instance.GameManager.SetLoadData(this);
    }

    public IEnumerator FetchInternetTime(Action onFetched)
    {
        DateTime internetTime = DateTime.Now;

        using (UnityWebRequest req = UnityWebRequest.Head("https://www.google.com"))
        {
            yield return req.SendWebRequest();

            string dateHeader = req.GetResponseHeader("date");
            if (DateTime.TryParse(dateHeader, out DateTime serverTime))
                internetTime = serverTime.ToLocalTime();
        }

        var difficultyName = ((Difficulty)difficulty).GetName();
        var chapterName = ((ChapterType)chapter).GetName();
        var result = internetTime.ToString("yy-MM-dd HH:mm:ss");
        saveName = $"[{difficultyName}]{chapterName}. {result}";
        onFetched?.Invoke();
    }
}