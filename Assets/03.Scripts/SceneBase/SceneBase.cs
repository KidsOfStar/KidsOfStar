using UnityEngine;
using UnityEngine.Events;

// 풀 생성, npc 넘겨주기 등 씬 초기화에 필요한 작업들을 담당
// 씬에는 반드시 SceneBase를 상속받은 베이스가 있어야 함
// 씬 고유 초기화 작업은 InitSceneExtra() 메서드에서 수행
public abstract class SceneBase : MonoBehaviour
{
    [Header("Chapter")]
    [SerializeField] private ChapterType currentChapter;
    
    [Header("Player Settings")]
    [SerializeField] private GameObject playerPrefab; // TODO: 리소스 로드 할지?
    [SerializeField] private Transform playerSpawnPosition;
    
    [Header("NPCs")]
    [SerializeField] private Npc[] speakers;

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    
    // TODO: 각 씬 별로 플레이어가 자유상호작용 때 말하는 부분이 있다면 플레이어도 스폰 후 speaker로 등록해야함
    private void Awake()
    {
        // 매니저들 초기화
        InitManagers();
        
        // 로딩중인 씬 매니저에게 씬이 활성화 되었음을 알림
        Managers.Instance.SceneLoadManager.IsSceneLoadComplete = true;

        // 게임 매니저에 현재 챕터를 설정
        InitSceneBase();
        
        // 씬이 로드된 후에 플레이어를 스폰
        SpawnPlayer();

        // 플레이어 스폰 후 카메라 컨트롤러의 타겟 설정
        InitCameraController();
        
        // 필수 UI 표시
        ShowRequiredUI();
        
        // 씬에 따라 추가적인 초기화 작업 수행
        InitSceneExtra();
    }

    private void InitManagers()
    {
        Managers.Instance.GameManager.SetCamera(mainCamera);
        Managers.Instance.OnSceneLoaded();
        Managers.Instance.DialogueManager.InitSceneNPcs(speakers);
    }
    
    private void SpawnPlayer()
    {
        var gameManager = Managers.Instance.GameManager;
        Vector3 playerPosition = gameManager.IsNewGame ? playerSpawnPosition.position : gameManager.PlayerPosition;
            
        GameObject player = Instantiate(playerPrefab, playerPosition, Quaternion.identity);
        Managers.Instance.GameManager.SetPlayer(player.GetComponent<Player>());
    }

    private void InitCameraController()
    {
        if(mainCamera.TryGetComponent(out CameraController cameraController))
            cameraController.Init();
        else
            Debug.LogError("SceneBase : CameraController not found on the main camera.");
    }
    
    private void ShowRequiredUI()
    {
        Managers.Instance.UIManager.Show<PlayerBtn>();
        Managers.Instance.UIManager.Show<UIJoystick>();
    }

    private void InitSceneBase()
    {
        Managers.Instance.GameManager.SetChapter(currentChapter);
    }

    // 씬 내에서 TriggerEnter로 진행도를 업데이트할 때 사용
    public void UpdateProgress()
    {
        Managers.Instance.GameManager.UpdateProgress();
    }
    
    protected abstract void InitSceneExtra();
}
