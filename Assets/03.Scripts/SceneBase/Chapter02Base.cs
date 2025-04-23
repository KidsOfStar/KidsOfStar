using System;

public class Chapter02Base : SceneBase
{
    protected override void InitSceneExtra(Action playIntroCallback)
    {
        Managers.Instance.SoundManager.PlayBgm(BgmSoundType.InForest);

        Managers.Instance.CutSceneManager.PlayCutScene(CutSceneType.Rescued.GetName());
    }
}
