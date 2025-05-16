using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafePuzzleSystem : TreePuzzleSystem
{
    public SafePuzzle safePuzzle;
    public SafePopup safePopup;
    public int index; // 퍼즐 고유 ID

    // 성공 시
    protected override void CompletePuzzle()
    {
        isRunning = false;
        Managers.Instance.SoundManager.PlaySfx(SfxSoundType.PuzzleClear);

        // 금고 다이얼 해금
        SafeSetActive(index);

        EditorLog.Log("퍼즐 성공!");
        if (!clearPuzzlenum.Contains(puzzleIndex))
        {
            clearPuzzlenum.Add(puzzleIndex);
        }

        // 클리어 시 다음 퍼즐로 이동
        safePopup.nextPuzzle();
    }

    // 실패 시
    protected override void FailPuzzle()
    {
        isRunning = false;
        Managers.Instance.SoundManager.PlaySfx(SfxSoundType.PuzzleFail);

        OnExit();

        // GameOverPopup 보여주고 다시 시작할지 물어봄
        Managers.Instance.UIManager.Show<GameOverPopup>();

        // 트리거 리셋
        if (triggerMap.TryGetValue(puzzleIndex, out var trig))
        {
            trig.ResetTrigger();
        }
    }


    public override void OnClearButtonClicked()
    {
        if (triggerMap.TryGetValue(puzzleIndex, out var trig))
        {
            trig.DisableExclamation();
        }

        // 팝업 닫고 플레이어 제어 복구
        OnExit();
    }

    public override void OnExit()
    {
        Managers.Instance.SoundManager.PlayBgm(BgmSoundType.InForest);
        Managers.Instance.UIManager.Hide<SafePopup>();
        Managers.Instance.GameManager.Player.Controller.UnlockPlayer();
    }


    private void SafeSetActive(int indexs)
    {
        safePuzzle.safeImage[indexs].raycastTarget = true; // 클릭 가능하게 설정
        safePuzzle.safeImage[indexs].color = Color.white; // 색상 변경
        index++;
    }

}
