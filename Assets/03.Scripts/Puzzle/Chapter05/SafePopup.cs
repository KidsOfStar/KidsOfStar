using System;
using System.Collections.Generic;
using UnityEngine;

public class SafePopup : PopupBase
{
    [SerializeField] private SafePuzzleSystem currentPuzzle;

    public Door door;

    [Header("퍼즐 시스템")]
    public TreePuzzleData[] datas;
    public SafePuzzleSystem[] puzzleSystems;

    public int countIndex = 0;

    protected override void Start()
    {
        door = GameObject.FindWithTag("Interactable").GetComponent<Door>();
        currentPuzzle = puzzleSystems[0];
        nextPuzzle();
    }

    public override void Opened(params object[] param)
    {
        base.Start();
        Managers.Instance.GameManager.Player.Controller.LockPlayer();
    }

    public void nextPuzzle()
    {
        if (countIndex >= datas.Length)
        {
            Debug.Log("모든 퍼즐이 완료되었습니다.");
            UpdateChapterProgress();
            door.isDoorLocked = true;
            Managers.Instance.UIManager.Show<ClearPuzzlePopup>();
            Managers.Instance.GameManager.Player.Controller.UnlockPlayer();
            return;
        }

        currentPuzzle.SetupPuzzle(datas[countIndex], 2);
        currentPuzzle.GeneratePuzzle();
        currentPuzzle.StartPuzzle();

        countIndex++;

        // 조건 수정
        if (countIndex < puzzleSystems.Length)
        {
            currentPuzzle = puzzleSystems[countIndex];
        }
    }

    public void ResetAllPuzzles()
    {
        countIndex = 0;

        foreach (var puzzleSystem in puzzleSystems)
        {
            puzzleSystem.ResetSystem();
        }

        currentPuzzle = puzzleSystems[0];
        Debug.Log("모든 퍼즐이 초기화되었습니다.");
    }

    public void UpdateChapterProgress()
    {
        Managers.Instance.GameManager.UpdateProgress();
        Debug.Log("모든 퍼즐이 완료되었습니다. 챕터 진행도를 올립니다.");
    }

    public void FullReset()
    {
        countIndex = 0;
        currentPuzzle = puzzleSystems[0];

        foreach (var puzzleSystem in puzzleSystems)
        {
            puzzleSystem.ResetSystem();
        }

        Debug.Log("SafePopup과 모든 퍼즐이 완전히 초기화되었습니다.");
    }
}
