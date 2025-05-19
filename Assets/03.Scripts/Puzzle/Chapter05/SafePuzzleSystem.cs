using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SafePuzzleSystem : TreePuzzleSystem
{
    public SafePuzzle safePuzzle;
    public SafePopup safePopup;
    public int index;

    // 퍼블 성공
    protected override void CompletePuzzle()
    {
        isRunning = false;
        Managers.Instance.SoundManager.PlaySfx(SfxSoundType.PuzzleClear);
        SafeSetActive(index);

        if (!clearPuzzlenum.Contains(puzzleIndex))
        {
            clearPuzzlenum.Add(puzzleIndex);
        }

        safePopup.nextPuzzle();
    }

    // 퍼즐 실패
    protected override void FailPuzzle()
    {
        isRunning = false;
        Managers.Instance.SoundManager.PlaySfx(SfxSoundType.PuzzleFail);
        OnExit(); // UI 닫고 플레이어 언락
        Managers.Instance.UIManager.Show<GameOverPopup>(); // 실패 팝업
        safePopup.FullReset(); // 퍼즐 시스템 리셋

        if (triggerMap.TryGetValue(puzzleIndex, out var trig))
        {
            trig.ResetTrigger(); // 트리거 리셋
        }
    }


    public override void OnClearButtonClicked()
    {
        if (triggerMap.TryGetValue(puzzleIndex, out var trig))
        {
            trig.DisableExclamation();
        }

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
        safePuzzle.safeImage[indexs].raycastTarget = true;
        safePuzzle.safeImage[indexs].color = Color.white;
        index++;
    }

    // 시스템 초기화
    public void ResetSystem()
    {
        index = 0;
        clearPuzzlenum.Clear();
        isRunning = false;

        foreach (var img in safePuzzle.safeImage)
        {
            img.raycastTarget = false;
            img.color = Color.gray;
        }

        safePuzzle.ResetPuzzleState(); // 퍼즐 상태 초기화
    }

    public override void SetupPuzzle(TreePuzzleData data, int puzzleClearIndex)
    {
        base.SetupPuzzle(data, puzzleClearIndex);
    }

}
