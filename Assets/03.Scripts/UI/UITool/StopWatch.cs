using TMPro;
using UnityEngine;

public class StopWatch : UIBase
{
    [Header("StopWatch")] // 스탑워치
    public float timeStrat; // 시작 시간
    public float targetTime; // 목표 시간   

    private bool isTiming = false; // 타이밍 여부

    public TextMeshProUGUI timeText; // UI 텍스트

    // Start is called before the first frame update
    void Start()
    {
        timeText.text = "00:00:00"; // 초기화
    }

    // Update is called once per frame
    void Update()
    {
        StartTime();
    }

    public void StartTime()
    {
        if(!isTiming) return; // 타이밍이 아닐 경우 종료
        timeStrat += Time.deltaTime; // 시간 증가
        //timeStrat = Mathf.Clamp(timeStrat, 0, targetTime); // 시간 제한
        int minutes = (int)(timeStrat / 60); // 분 계산
        int seconds = (int)(timeStrat % 60); // 초 계산
        int milliseconds = (int)((timeStrat - (int)timeStrat) * 100); // 밀리초 계산

        timeText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", minutes, seconds, milliseconds); // 텍스트 업데이트
    }

    public void OnStartWatch()
    {
        // 스탑워치 시동
        isTiming = true; // 타이밍 시작
    }

    public void OnStopWatch()
    {
        // 스탑워치 시간 정지
        isTiming = false; // 타이밍 종료
    }

    public void ResetTime() // 시간 초기화 메소드
    {
        timeStrat = 0; // 시간 초기화
        timeText.text = "00:00:00"; // UI 텍스트 초기화
    }

    public void CheckTargetTime()
    {
        // 도착했는데 목표 시간보다 늦을 경우 다시하기
        if (timeStrat >= targetTime)
        {
            ResetTime(); // 시간 초기화 메소드 호출
        }
        // 목표 시간에 빨리 들어왔을 경우
        else if(timeStrat <= targetTime)
        {
            timeStrat = 0; // 시간 초기화
            timeText.text = "00:00:00"; // UI 텍스트 초기화
        }
    }
}
