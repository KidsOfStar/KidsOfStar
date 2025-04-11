public class Chapter01 : SceneBase
{
    protected override void Awake()
    {
        base.Awake();
        SpawnPlayer();
        
        // SceneLoadManager의 완료 상태를 설정
        Managers.Instance.SceneLoadManager.IsSceneLoadComplete = true;
    }
}
