public class Chapter02Base : SceneBase
{
    protected override void InitSceneExtra()
    {
        Managers.Instance.SoundManager.PlayBgm(BgmSoundType.InForest);
        Managers.Instance.CutSceneManager.PlayCutScene(CutSceneType.Chapter2.GetName());
    }
}
