using Unity.Services.Core;
using Unity.Services.Analytics;

public class AnalyticsManager
{
    // Chapter 103
    public int TryCount { get; set; }
    
    // Chapter 2, 4
    public int fallCount;

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

    public void RecordChapterEvent(
        string eventType,
        params (string key, object value)[] parameters
    )
    {
        // 1) CustomEvent 객체 생성
        var ce = new CustomEvent(eventType);

        // 2) 공통 파라미터: Chapter 번호
        ce["ChapterType"] = Managers.Instance.GameManager.CurrentChapter.GetName();

        // 3) 추가 파라미터 병합
        foreach (var (key, value) in parameters)
            ce[key] = value;

        // 4) Analytics 전송
        AnalyticsService.Instance.RecordEvent(ce);
    }

    public void SendFunnel(string number)
    {
        var funnelEvent = new CustomEvent("Funnel_Chapter");
        funnelEvent["Funnel_Chapter_Number"] = number;
        
        AnalyticsService.Instance.RecordEvent(funnelEvent);
    }
}



