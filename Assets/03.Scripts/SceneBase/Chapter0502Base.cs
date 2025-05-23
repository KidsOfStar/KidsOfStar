using System;

public class Chapter0502Base : SceneBase
{
    protected override void CutSceneEndCallback() { }

    protected override void InitSceneExtra(Action callback)
    {
        Managers.Instance.SoundManager.PlayBgm(BgmSoundType.Aquarium);
        Managers.Instance.SoundManager.PlayAmbience(AmbienceSoundType.Aquarium);

        if (isFirstTime)
            Managers.Instance.GameManager.UpdateProgress(); // 진행도 2
    }
}