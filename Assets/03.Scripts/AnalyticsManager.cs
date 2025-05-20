using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Net.NetworkInformation;

public class AnalyticsManager 
{
    public AnalyticsManager()
    {
        Init();
    }

    async void Init()
    {
        try
        {
            //Unity Services 초기화
            await UnityServices.InitializeAsync();
            AnalyticsService.Instance.StartDataCollection();

        }
        catch (System.Exception error)
        {
            EditorLog.Log($"Unity Services failed to + {error}");
        }
    }

    public static void RecordChapterEvent(
        string eventType,
        params (string key, object value)[] parameters
    )
    {
        // 1) CustomEvent 객체 생성
        var ce = new CustomEvent(eventType);

        // 2) 공통 파라미터: Chapter 번호
        ce["Chapter"] = (int)Managers.Instance.GameManager.CurrentChapter;

        // 3) 추가 파라미터 병합
        foreach (var (key, value) in parameters)
            ce[key] = value;

        // 4) Analytics 전송
        AnalyticsService.Instance.RecordEvent(ce);
    }
}



