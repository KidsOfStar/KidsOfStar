using System;
using System.Collections.Generic;
using UnityEngine;

public class Chapter0501Base : SceneBase
{
    public bool istutorialForm = false;

    [SerializeField] List<PuzzleTriggerBase> puzzleTriggers;

    protected override void InitSceneExtra(Action callback)
    {
        SkillForm();
        Managers.Instance.SoundManager.PlayBgm(BgmSoundType.Aquarium);
        Managers.Instance.SoundManager.PlayAmbience(AmbienceSoundType.Aquarium);
        callback?.Invoke();

        var puzzleManager = Managers.Instance.PuzzleManager;
        puzzleManager.OnSceneLoaded();
        foreach (var trigger in puzzleTriggers)
        {
            trigger?.InitTrigger();
            trigger?.SetupUI();
        }
    }
    
    protected override void CutSceneEndCallback()
    {
        PlayChapterIntro(HideTutorial);
        SkillForm();
    }

    private void SkillForm()
    {
        Managers.Instance.GameManager.UnlockForm(PlayerFormType.Hide);
    }

    private void HideTutorial()
    {
        if (istutorialForm)
        {
            var popup = Managers.Instance.UIManager.Show<TutorialPopup>(3);
            istutorialForm = true;
        }
    }
}

