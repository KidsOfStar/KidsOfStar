using UnityEngine;

public class Managers : Singleton<Managers>
{
    [field: SerializeField] public SceneLoadManager SceneLoadManager { get; private set; }
    [field: SerializeField] public DialogInputHandler DialogInputHandler { get; private set; }
    
    public ResourceManager ResourceManager { get; private set; }
    public DataManager DataManager { get; private set; }
    public PoolManager PoolManager { get; private set; }
    public SoundManager SoundManager { get; private set; }
    public DialogueManager DialogueManager { get; private set; }
    public GameManager GameManager { get; private set; }
    public UIManager UIManager { get; private set; }

    // TODO: 생성자랑 Init이랑 나눌까?
    protected override void Awake()
    {
        base.Awake();
        ResourceManager = new ResourceManager();
        DataManager = new DataManager();
        PoolManager = new PoolManager();
        SoundManager = new SoundManager();
        DialogueManager = new DialogueManager();
        GameManager = new GameManager();
		UIManager = new UIManager();
        DialogInputHandler.gameObject.SetActive(false);

        InitManagers();
    }

    public void InitManagers()
    {
        DataManager.Init();
        GameManager.Init();
        SoundManager.Init(); // TODO: 씬 로드 시마다 필요
        UIManager.Init();
        DialogueManager.Init();
    }
}