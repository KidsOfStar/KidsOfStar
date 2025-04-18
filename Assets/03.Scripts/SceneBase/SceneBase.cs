using UnityEngine;
using UnityEngine.Events;

// 풀 생성, npc 넘겨주기 등 씬 초기화에 필요한 작업들을 담당
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
    
    [Header("Events")]
    [SerializeField] protected UnityEvent onLoadComplete;
    
    // TODO: 각 씬 별로 플레이어가 자유상호작용 때 말하는 부분이 있다면 플레이어도 스폰 후 speaker로 등록해야함
    protected virtual void Awake()
    {
        // 매니저들 초기화
        InitManagers();
        
        // 로딩중인 씬 매니저에게 씬이 활성화 되었음을 알림
        Managers.Instance.SceneLoadManager.IsSceneLoadComplete = true;

        // 씬이 로드된 후에 플레이어를 스폰
        SpawnPlayer();
        
        // 씬에 미리 배치된 오브젝트들의 초기화
        onLoadComplete?.Invoke();
        
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
    
    private void ShowRequiredUI()
    {
        Managers.Instance.UIManager.Show<PlayerBtn>();
        Managers.Instance.UIManager.Show<UIJoystick>();
    }

    private void InitSceneBase()
    {
        Managers.Instance.GameManager.SetChapter(currentChapter);
    }

    public void UpdateProgress()
    {
        Managers.Instance.GameManager.UpdateProgress();
    }
    
    protected abstract void InitSceneExtra();
}
