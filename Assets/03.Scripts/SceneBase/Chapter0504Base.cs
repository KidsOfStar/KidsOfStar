using System;

public class Chapter0504Base : SceneBase
{
    protected override void CutSceneEndCallback() { }
    protected override void InitSceneExtra(Action callback) {
        Managers.Instance.SoundManager.PlayBgm(BgmSoundType.Aquarium);
        Managers.Instance.SoundManager.PlayAmbience(AmbienceSoundType.Aquarium);

        Managers.Instance.GameManager.UnlockForm(PlayerFormType.Dog);
        Managers.Instance.GameManager.UnlockForm(PlayerFormType.Squirrel);
        Managers.Instance.GameManager.UnlockForm(PlayerFormType.Cat);
        Managers.Instance.GameManager.UnlockForm(PlayerFormType.Hide);

    }
}