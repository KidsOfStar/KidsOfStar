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
    
    [Header("Player Position")]
    [Tooltip("컷신 이후 플레이어 위치를 잡는 부모오브젝트")]
    [SerializeField] private PlayerSpawnPointer spawnPointer;
    [SerializeField] private ScrollingBackGround scrollingBackGround;

    private Action onCutSceneEndHandler;

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
        // 챕터별 컷신이 필요한 경우 컷씬 재생 이후 Bgm, Intro 등이 재생되도록 콜백을 등록
        InitSceneExtra(ChapterCutSceneCallback);

        // 컷신 종료 후 이동 할 플레이어 위치 정보 설정
        if (spawnPointer) spawnPointer.Init();

        InitBg();
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
            Managers.Instance.PoolManager.CreatePool(prefab, 5);
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
        
        if (Managers.Instance.GameManager.IsNewGame)
            Managers.Instance.GameManager.ResetProgress();
        else
        {
            isFirstTime = false;
            Managers.Instance.GameManager.SetLoadedProgress();
        }
    }

    // 씬 내에서 TriggerEnter로 진행도를 업데이트할 때 사용
    public void UpdateProgress()
    {
        Managers.Instance.GameManager.UpdateProgress();
    }

    protected void PlayChapterIntro()
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

    private void InitBg()
    {
        scrollingBackGround.Initialized(mainCamera.transform);
    }

    protected abstract void InitSceneExtra(Action callback);

    protected abstract void ChapterCutSceneCallback();
    
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