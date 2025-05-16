using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafePuzzleSystem : TreePuzzleSystem
{
    public SafePuzzle safePuzzle;
    public SafePopup safePopup;
    public int index;

    protected override void CompletePuzzle()
    {
        isRunning = false;
        Managers.Instance.SoundManager.PlaySfx(SfxSoundType.PuzzleClear);
        SafeSetActive(index);

        EditorLog.Log("퍼즐 성공!");
        if (!clearPuzzlenum.Contains(puzzleIndex))
        {
            clearPuzzlenum.Add(puzzleIndex);
        }

        safePopup.nextPuzzle();
    }

    protected override void FailPuzzle()
    {
        isRunning = false;
        Managers.Instance.SoundManager.PlaySfx(SfxSoundType.PuzzleFail);
        OnExit();
        Managers.Instance.UIManager.Show<GameOverPopup>();
        safePopup.FullReset();

        if (triggerMap.TryGetValue(puzzleIndex, out var trig))
        {
            trig.ResetTrigger();
        }
    }

    public void ResetPuzzle()
    {
        isRunning = false;
        safePopup.FullReset();
        Managers.Instance.UIManager.Hide<SafePopup>();

        if (triggerMap.TryGetValue(puzzleIndex, out var trig))
        {
            trig.ResetTrigger();
        }

        EditorLog.Log("퍼즐이 초기화되었습니다. 다시 시도할 수 있습니다.");
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
}
