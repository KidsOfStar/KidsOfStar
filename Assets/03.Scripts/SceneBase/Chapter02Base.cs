public class Chapter02Base : SceneBase
{
    protected override void InitSceneExtra()
    {
        Managers.Instance.SoundManager.PlayBgm(BgmSoundType.InForest);
        Managers.Instance.CutSceneManager.PlayCutScene(CutSceneType.Rescued.GetName());
        //Managers.Instance.UIManager.Show<TreePuzzlePopup>();
        //Managers.Instance.UIManager.Hide<TreePuzzlePopup>();

    }
}
