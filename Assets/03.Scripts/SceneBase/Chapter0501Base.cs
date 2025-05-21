using System;

public class Chapter0501Base : SceneBase
{
    public bool istutorialForm = false;
    protected override void CutSceneEndCallback()
    {
        PlayChapterIntro();
    }

    protected override void InitSceneExtra(Action callback)
    {
        Managers.Instance.SoundManager.PlayBgm(BgmSoundType.Aquarium);
        Managers.Instance.SoundManager.PlayAmbience(AmbienceSoundType.Aquarium);

        if (istutorialForm)
        {
            var popup = Managers.Instance.UIManager.Show<TutorialPopup>(3);
            istutorialForm = true;
        }

        SkillForm();
    }

    private void SkillForm()
    {
        Managers.Instance.GameManager.UnlockForm(PlayerFormType.Hide);
        Managers.Instance.GameManager.UnlockForm(PlayerFormType.Squirrel);
        Managers.Instance.GameManager.UnlockForm(PlayerFormType.Dog);
        Managers.Instance.GameManager.UnlockForm(PlayerFormType.Cat);
    }
}


