using Unity.Services.Core;
using Unity.Services.Analytics;

public class AnalyticsManager
{
    // Chapter 103
    public int ChaseTryCount { get; set; }
    
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
}

// 1. 사용해야할 CustomEventName을 찾는다.
// 2. var analyticsManager = Managers.Instance.AnalyticsManager; 를 선언
// 3. RecordEvent를 등록하기 위해 메서드를 호출
// 4. 등록해야하는 데이터와 파라미터를 연결
// 5. 연결방법
// analyticsManager.RecordChapterEvent("CustomEventName",
//                                    ("ParameterName", 해당씬에서 연결해야할 변수)

// **ChapterType 파라미터는 공통적으로 연결되므로 따로 작성안해도 됨.

// 사용예시
// analyticsManager.RecordChapterEvent("Choice",
//                                    ("Choice", dialogData.SelectOption[selectIndex]),
//                                    ("Index", dialogData.Index));


