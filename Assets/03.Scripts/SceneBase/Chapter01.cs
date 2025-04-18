public class Chapter01 : SceneBase
{
    // 씬 베이스를 상속받은 친구들이 강제로 구현할 수 있도록 수정
    protected override void Awake()
    {
        base.Awake();
        // 로딩중인 씬 매니저에게 씬이 활성화 되었음을 알림
        Managers.Instance.SceneLoadManager.IsSceneLoadComplete = true;

        // 씬이 로드된 후에 플레이어를 스폰
        // SpawnPlayer();
        
        // 씬에 미리 배치된 오브젝트들의 초기화
        /// Camera의 SetTarget은 플레이어를 타겟으로 하기 때문에 반드시 플레이어 스폰 후에 호출해야함
        onLoadComplete?.Invoke();
        
        Managers.Instance.CutSceneManager.PlayCutScene(CutSceneType.Chapter02_Test.GetName());
    }
}

