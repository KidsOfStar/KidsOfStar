using System.Collections;
using UnityEngine;

public class TestGame : MonoBehaviour
{
    public StopWatch stopWatch;
    public CountDownPopup countDownPopup;

    // Start is called before the first frame update
    void Start()
    {
        countDownPopup = Managers.Instance.UIManager.Show<CountDownPopup>();
        Managers.Instance.UIManager.Hide<CountDownPopup>(); // 카운트다운 팝업 숨김
        stopWatch = Managers.Instance.UIManager.Show<StopWatch>();
        Managers.Instance.UIManager.Hide<StopWatch>(); // 스탑워치 숨김
    }

    public void StartTest()
    {
        // 시작할 때
        Managers.Instance.UIManager.Show<CountDownPopup>(); // 카운트다운 팝업 표시
        countDownPopup.CountDownStart(); // 카운트다운 시작

        StartCoroutine(StartGame(5f)); // 카운트다운 대기 후 게임 시작
        Managers.Instance.UIManager.Show<StopWatch>(); // 스탑워치 표시

    }

    private IEnumerator StartGame(float delay)
    {
        yield return new WaitForSeconds(delay); // 카운트다운 대기
        stopWatch.OnStartWatch(); // 스탑워치 시작
        stopWatch.StartTime(); // 스탑워치 시간 시작
    }
    public void StopTest()
    {
        // 종료할 때
        stopWatch.OnStopWatch(); // 스탑워치 정지
        stopWatch.CheckTargetTime(); // 목표 시간 체크
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 상호작용 버튼 이벤트에 등록
        if (collision.CompareTag("Player"))
        {
            StartTest(); // 테스트 시작
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 상호작용 버튼 이벤트에 해제
        if (!collision.CompareTag("Player"))
        {
            StopTest();
        }
    }
}

