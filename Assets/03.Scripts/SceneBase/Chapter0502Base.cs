using System;
using System.Collections.Generic;
using UnityEngine;

public class Chapter0502Base : SceneBase
{
    [SerializeField] List<PuzzleTriggerBase> puzzleTriggers;

    protected override void CutSceneEndCallback() { }

    protected override void InitSceneExtra(Action callback)
    {
        Managers.Instance.SoundManager.PlayBgm(BgmSoundType.Aquarium);
        Managers.Instance.SoundManager.PlayAmbience(AmbienceSoundType.Aquarium);

        var puzzleManager = Managers.Instance.PuzzleManager;
        puzzleManager.OnSceneLoaded();
        foreach (var trigger in puzzleTriggers)
        {
            trigger?.InitTrigger();
            trigger?.SetupUI();
        }

        if (isFirstTime)
            Managers.Instance.GameManager.UpdateProgress(); // 진행도 2
        else
            Managers.Instance.GameManager.SetLoadedProgress();
    }

    private void Update()
    {
        Debug.Log($"{Managers.Instance.GameManager.ChapterProgress}");
    }
}