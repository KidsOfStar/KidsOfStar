using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class DashGame : MonoBehaviour
{
    public StopWatch stopWatch;
    public CountDownPopup countDownPopup;
    public PlayerController playerController;

    public float playerSpeed; // 플레이어 속도
    public bool isGameStarted = false;
    //public float targetTime; // 목표 시간

    public List<float> targetTimeList;  // 목표 시간 리스트

    private SkillBTN skillBTN; // 스킬 버튼 UI

    // Start is called before the first frame update
    void Start()
    {
        skillBTN = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel; // 스킬 버튼 UI 가져오기

        playerController = Managers.Instance.GameManager.Player.Controller;
        playerSpeed = playerController.MoveSpeed; // 플레이어 속도 초기화

        countDownPopup = Managers.Instance.UIManager.Show<CountDownPopup>();
        Managers.Instance.UIManager.Hide<CountDownPopup>(); // 카운트다운 팝업 숨김

        stopWatch = Managers.Instance.UIManager.Show<StopWatch>();
        Managers.Instance.UIManager.Hide<StopWatch>(); // 스탑워치 숨김
    }

    public void StartGame()
    {
        if (isGameStarted) return; // 이미 게임이 시작된 경우 종료
        isGameStarted = true; // 게임 시작 상태로 변경

        // 플레이어 속도 0으로 하여 정지
        playerController.MoveSpeed = 0; // 플레이어 속도 0으로 설정

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
        playerController.MoveSpeed = playerSpeed * 1.5f; // 플레이어 속도 초기화 (1.5배 증가)
    }

    public void EndGame(NPCType npcType)
    {
        if (!isGameStarted) return;

        stopWatch.OnStopWatch();
        playerController.MoveSpeed = playerSpeed;

        float clearTime = stopWatch.recodeTime;

        ShowDialogueResult(clearTime, npcType); // 대사 출력
    }

    private void ShowDialogueResult(float clearTime, NPCType npcType)
    {
        if (Managers.Instance.UIManager.Get<DashGameResultPopup>())
        {
            Managers.Instance.UIManager.Get<DashGameResultPopup>().OnClickDialogue();
        }
        else
        {
            if (stopWatch.recodeTime < 90f)
            {
                Managers.Instance.UIManager.Show<DashGameResultPopup>(0f, npcType).OnClickDialogue();
            }
            else if (stopWatch.recodeTime < 210f)
            {
                Managers.Instance.UIManager.Show<DashGameResultPopup>(1f, npcType).OnClickDialogue();
            }
            else
            {
                Managers.Instance.UIManager.Show<DashGameResultPopup>(2f, npcType).OnClickDialogue();
            }
        }
    }
}