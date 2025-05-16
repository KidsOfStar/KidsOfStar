using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafePuzzleSystem : TreePuzzleSystem
{
    public SafePuzzle safePuzzle;
    public SafePopup safePopup;
    public int index; // 퍼즐 고유 ID
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
