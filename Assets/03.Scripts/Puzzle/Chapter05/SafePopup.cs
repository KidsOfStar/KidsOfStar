using System;
using System.Collections.Generic;
using UnityEngine;

public class SafePopup : PopupBase
{
    /// Chapter 5에서 사용되는 퍼즐 시스템
    [SerializeField] private SafePuzzleSystem currentPuzzle;

    public TreePuzzleData[] datas;
    public SafePuzzleSystem[] puzzleSystems;


    public int countIndex = 0; // 퍼즐 조각의 인덱스

    [Header("퍼즐 시스템")]
    public bool elevatorPuzzle = false;
    public bool safePuzzle = false;

    public override void Opened(params object[] param)
    {
        base.Start();
        HideUI(false); // UI 비활성화
        LockPlayer(true); // 플레이어 잠금

        currentPuzzle = puzzleSystems[0];

        nextPuzzle();

    }
    public void nextPuzzle()
    {
        if (countIndex >= datas.Length)
        {
            Debug.Log("모든 퍼즐이 완료되었습니다.");
            return;
        }
        currentPuzzle.SetupPuzzle(datas[countIndex], 2);
        currentPuzzle.GeneratePuzzle();
        currentPuzzle.StartPuzzle();

        // 퍼즐이 완료되면 다음 퍼즐로 이동
        countIndex++;


        if (countIndex > puzzleSystems.Length)
        {
            currentPuzzle = puzzleSystems[countIndex];
        }
    }

    protected override void LockPlayer(bool lockPlayer)
    {
        base.LockPlayer(lockPlayer);
    }

    // 모든 퍼즐이 완료되었는지 확인하는 메서드
    public bool IsAllPuzzlesCompleted()
    {
        return elevatorPuzzle && safePuzzle;
    }

    // 모든 퍼즐이 완료가 되면 챕터 진행도 올리기
    public void UpdateChapterProgress()
    {
        if (IsAllPuzzlesCompleted())
        {
            // 챕터 진행도 올리기
            Managers.Instance.GameManager.UpdateProgress();
            Debug.Log("모든 퍼즐이 완료되었습니다. 챕터 진행도를 올립니다.");
        }
    }
}
