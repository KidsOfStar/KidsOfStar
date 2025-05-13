using System;
using UnityEngine.Rendering;


public class Chapter0501Base : SceneBase
{
    protected override void CutSceneEndCallback()
    {
        PlayChapterIntro();
    }

    protected override void InitSceneExtra(Action playIntroCallback)
    {
        SkillForm();
        playIntroCallback?.Invoke();
    }

    private void SkillForm()
    {
        Managers.Instance.GameManager.UnlockForm(PlayerFormType.Hide);
    }
}


