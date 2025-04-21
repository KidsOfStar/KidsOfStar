using TMPro;
using UnityEngine;

public class StopWatch : MonoBehaviour
{
    [Header("StopWatch")] // 스탑워치
    public float timeStrat; // 시작 시간
    public float targetTime; // 목표 시간   
    public TextMeshProUGUI timeText; // UI 텍스트

    // Start is called before the first frame update
    void Start()
    {
        timeStrat = 0; // 시작 시간 초기화
        targetTime = 10f; // 목표 시간 설정 (10초)
        timeText.gameObject.SetActive(true); // 텍스트 활성화
        timeText.text = "00:00:000"; // 초기 텍스트 설정
    }

    // Update is called once per frame
    void Update()
    {
        StartTime();
    }

    private void StartTime()
    {
        timeStrat += Time.deltaTime; // 시간 증가
        int minutes = (int)(timeStrat / 60); // 분 계산
        int seconds = (int)(timeStrat % 60); // 초 계산
        int milliseconds = (int)((timeStrat - Mathf.Floor(timeStrat)) * 1000); // 밀리초 계산
        timeText.text = string.Format("{0:D2}:{1:D2}:{2:D3}", minutes, seconds, milliseconds); // UI 텍스트 업데이트

        if (timeStrat >= targetTime) // 목표 시간 도달 시
        {
            timeText.gameObject.SetActive(false); // 텍스트 숨기기
            gameObject.SetActive(false); // 오브젝트 비활성화
        }
    }

    public void ResetTime() // 시간 초기화 메소드
    {
        timeStrat = 0; // 시작 시간 초기화
        timeText.text = "00:00:000"; // UI 텍스트 초기화
        gameObject.SetActive(true); // 오브젝트 활성화
    }
}
