using System;
using UnityEngine;

// 풀 생성, npc 넘겨주기 등 씬 초기화에 필요한 작업들을 담당
// 씬에는 반드시 SceneBase를 상속받은 베이스가 있어야 함
// 씬 고유 초기화 작업은 InitSceneExtra() 메서드에서 수행
public abstract class SceneBase : MonoBehaviour
{
    [Header("Chapter")]
    [SerializeField] private ChapterType currentChapter;
    [SerializeField] private bool existRequiredDialog = true;
    [SerializeField] private bool isFirstTime = true;
    [SerializeField, TextArea] private string introText; // TODO: first Time은 어떻게 설정하지?

    [Header("Player Settings")]
    [SerializeField] private GameObject playerPrefab; // TODO: 리소스 로드 할지?

    [SerializeField] protected string playerStartForm;
    [SerializeField] private Transform playerSpawnPosition;

    [Header("NPCs")]
    [SerializeField] private SceneNpc[] speakers;

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;

    private Action onCutSceneEndHandler;

    [Tooltip("컷신 이후 플레이어 위치를 잡는 부모오브젝트")]
    [Header("Player Position")]

    [SerializeField] private PlayerSpawnPointer spawnPointer;
    // TODO: 각 씬 별로 플레이어가 자유상호작용 때 말하는 부분이 있다면 플레이어도 스폰 후 speaker로 등록해야함
    private void Awake()
    {
        // 매니저들 초기화
        InitManagers();

        CreatePool();
        
        // 로딩중인 씬 매니저에게 씬이 활성화 되었음을 알림
        Managers.Instance.SceneLoadManager.IsSceneLoadComplete = true;

#if UNITY_EDITOR
        Managers.Instance.LoadTestScene = false;
#endif
        
        // 게임 매니저에 현재 챕터를 설정
        InitSceneBase();

        // 씬이 로드된 후에 플레이어를 스폰
        SpawnPlayer();

        // 플레이어 스폰 후 카메라 컨트롤러의 타겟 설정
        InitCameraController();

        // 필수 UI 표시
        ShowRequiredUI();

        // 씬 고유 초기화 작업
        InitSceneExtra(PlayChapterIntro);

        // 컷신 플레이어 위치 정보 설정
        if (spawnPointer != null)
        {
            spawnPointer.Init();
        }
    }

    private void InitManagers()
    {
        Managers.Instance.GameManager.SetCamera(mainCamera);
        Managers.Instance.OnSceneLoaded();
        Managers.Instance.DialogueManager.InitSceneNPcs(speakers);
    }

    private void CreatePool()
    {
        // 필수 대화 아이콘 풀 생성
        if (existRequiredDialog)
        {
            const string path = Define.uiPath + Define.requiredIconKey;
            var prefab = Managers.Instance.ResourceManager.Load<GameObject>(path);
            Managers.Instance.PoolManager.CreatePool(prefab, 10);
        }
    }

    private void SpawnPlayer()
    {
        var gameManager = Managers.Instance.GameManager;
        Vector3 playerPosition = gameManager.IsNewGame ? playerSpawnPosition.position : gameManager.PlayerPosition;

        GameObject playerObj = Instantiate(playerPrefab, playerPosition, Quaternion.identity);
        Player player = playerObj.GetComponent<Player>();
        player.Init(playerStartForm);

        Managers.Instance.GameManager.SetPlayer(player);
        Managers.Instance.DialogueManager.SetPlayerSpeaker(player);

        onCutSceneEndHandler += () =>
        {
            playerObj.transform.position = playerSpawnPosition.position;
            playerObj.transform.rotation = playerSpawnPosition.rotation;
        };
        Managers.Instance.CutSceneManager.SetPlayerReferences(playerObj.transform, playerSpawnPosition);
        Managers.Instance.CutSceneManager.OnCutSceneEnd += onCutSceneEndHandler;
    }

    private void InitCameraController()
    {
        if (mainCamera.TryGetComponent(out CameraController cameraController))
            cameraController.Init();
        else
            EditorLog.LogError("SceneBase : CameraController not found on the main camera.");
    }

    private void ShowRequiredUI()
    {
        Managers.Instance.UIManager.Show<UIJoystick>();
        Managers.Instance.UIManager.Show<PlayerBtn>().Init();
    }

    private void InitSceneBase()
    {
        Managers.Instance.GameManager.SetChapter(currentChapter);
        
        // 챕터 변경 후 Npc 초기화
        InitNpc();
        
        // 이후 진행도 리셋을 통해 이벤트 호출
        Managers.Instance.GameManager.ResetProgress();
        
        // TODO: 이어하기가 있기 때문에 뉴 게임인지 검사해서 진행도 이벤트 호출
        // TODO: 뉴 게임이라면? 챕터 첫 진입이라면? -> ResetProgress
        // TODO: 이어하기라면? 게임매니저 데이터의 진행도를 가져와서 OnUpdateProgress?.Invoke() 
    }

    // 씬 내에서 TriggerEnter로 진행도를 업데이트할 때 사용
    public void UpdateProgress()
    {
        Managers.Instance.GameManager.UpdateProgress();
    }

    private void PlayChapterIntro()
    {
        var intro = Managers.Instance.UIManager.Show<UIChapterIntro>();
        StartCoroutine(intro.IntroCoroutine(isFirstTime, introText));
    }

    private void InitNpc()
    {
        foreach (var speaker in speakers)
        {
            speaker.Init();
        }
    }

    // playIntroCallback은 씬 진입 시 보여줄 인트로 UI 재생의 콜백
    // 컷씬 재생이 필요한 경우에는 이 콜백을 사용하여 컷씬 재생 후 인트로를 플레이
    protected abstract void InitSceneExtra(Action playIntroCallback);

    private void OnDestroy()
    {
        // 씬이 파괴될 때 반드시 구독 해제
        if (onCutSceneEndHandler != null)
        {
            Managers.Instance.CutSceneManager.OnCutSceneEnd -= onCutSceneEndHandler;
            onCutSceneEndHandler = null;
        }
    }
}

