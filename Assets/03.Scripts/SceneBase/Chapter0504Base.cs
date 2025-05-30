using System;
using System.Collections.Generic;
using UnityEngine;

public class Chapter0504Base : SceneBase
{
    [SerializeField] List<PuzzleTriggerBase> puzzleTriggers;

    private void Start()
    {
        var gm = Managers.Instance.GameManager;
        
        if (gm.SavePoint == 1)
        {
            gm.ChapterProgress = 3; // 504 진입 시 ChapterProgress를 3으로 설정
        }

    }

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

    }
}