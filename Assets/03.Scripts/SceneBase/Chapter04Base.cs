using System;

public class Chapter04Base : SceneBase
{
    protected override void InitSceneExtra(Action callback)
    {
        callback?.Invoke();
    }
    
    protected override void CutSceneEndCallback()
    {
        PlayChapterIntro();
        Managers.Instance.SoundManager.PlayBgm(BgmSoundType.City);
        Managers.Instance.SoundManager.PlayAmbience(AmbienceSoundType.City);
    }
}
