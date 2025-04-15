using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class SaveData
{
    // 저장할 때 이름 : 챕터 + 날짜 + 시간
    // 현재 챕터, 현재 챕터 진행도
    // 플레이어 위치
    // 챕터 신뢰도
    // 형태변환 해금 진행도
    // 지금까지 본 엔딩

    public string timeStamp;
    public int chapter;
    public int chapterProgress;
    public Vector3 playerPosition;
    public int[] chapterTrust;
    public bool[] endingDatas;

    public void InitData()
    {
        var gameManager = Managers.Instance.GameManager;
        chapter = (int)gameManager.CurrentChapter;
        chapterProgress = gameManager.ChapterProgress;
        playerPosition = gameManager.PlayerPosition;
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

        var chapterName = ((ChapterType)chapter).GetName();
        var result = internetTime.ToString("yy-MM-dd HH:mm:ss");
        timeStamp = $"{chapterName}. {result}";
        onFetched?.Invoke();
    }
}