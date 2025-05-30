using System;
using System.Collections.Generic;
using UnityEngine;

public class Chapter0504Base : SceneBase
{
    [SerializeField] List<PuzzleTriggerBase> puzzleTriggers;

    protected override void CutSceneEndCallback() { }
    protected override void InitSceneExtra(Action callback) 
    {
        var puzzleManager = Managers.Instance.PuzzleManager;
        puzzleManager.OnSceneLoaded();
        foreach (var trigger in puzzleTriggers)
        {
            trigger?.InitTrigger();
            trigger?.SetupUI();
        }
    }
}