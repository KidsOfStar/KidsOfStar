public class Chapter01 : SceneBase
{
    protected override void InitSceneExtra()
    {
        Managers.Instance.CutSceneManager.PlayCutScene(CutSceneType.Intro.GetName());
    }
}