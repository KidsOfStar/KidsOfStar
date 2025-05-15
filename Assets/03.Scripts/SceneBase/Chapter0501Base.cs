using System;
using UnityEngine.Rendering;


public class Chapter0501Base : SceneBase
{
    protected override void CutSceneEndCallback()
    {
        PlayChapterIntro();
    }

    protected override void InitSceneExtra(Action callback)
    {
        Managers.Instance.SoundManager.PlayBgm(BgmSoundType.Aquarium);
        Managers.Instance.SoundManager.PlayAmbience(AmbienceSoundType.Aquarium);
        SkillForm();
    }

    private void SkillForm()
    {
        Managers.Instance.GameManager.UnlockForm(PlayerFormType.Squirrel);

        Managers.Instance.GameManager.UnlockForm(PlayerFormType.Dog);

        Managers.Instance.GameManager.UnlockForm(PlayerFormType.Cat);

        Managers.Instance.GameManager.UnlockForm(PlayerFormType.Hide);
    }
}


