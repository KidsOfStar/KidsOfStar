using System;
using UnityEngine.Rendering;


public class Chapter0501Base : SceneBase
{
    protected override void ChapterCutSceneCallback()
    {
        PlayChapterIntro();
    }

    protected override void InitSceneExtra(Action callback)
    {
        SkillForm();
    }

    private void SkillForm()
    {
        Managers.Instance.GameManager.UnlockForm(PlayerFormType.Hide);
    }
}


