public class Chapter01Base : SceneBase
{
    protected override void InitSceneExtra()
    {
        Managers.Instance.CutSceneManager.PlayCutScene(CutSceneType.Intro.GetName());
    }
}