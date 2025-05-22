using System;

public class Chapter0504Base : SceneBase
{
    protected override void CutSceneEndCallback() { }
    protected override void InitSceneExtra(Action callback) {
        Managers.Instance.SoundManager.PlayBgm(BgmSoundType.Aquarium);
        Managers.Instance.SoundManager.PlayAmbience(AmbienceSoundType.Aquarium);
    }
}