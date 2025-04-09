using UnityEngine;

public class Managers : Singleton<Managers>
{
    [field: SerializeField] public SceneLoadManager SceneLoadManager { get; private set; }
    [field: SerializeField] public DialogInputHandler DialogInputHandler { get; private set; }
    
    public ResourceManager ResourceManager { get; private set; }
    public DataManager DataManager { get; private set; }
    public PoolManager PoolManager { get; private set; }
    public UIManager UIManager { get; private set; }
    public SoundManager SoundManager { get; private set; }
    public DialogueManager DialogueManager { get; private set; }
    public GameManager GameManager { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        ResourceManager = new ResourceManager();
        DataManager = new DataManager();
        PoolManager = new PoolManager();
        GameManager = new GameManager();
		UIManager = new UIManager();
        SoundManager = new SoundManager();
        DialogueManager = new DialogueManager();
        DialogInputHandler.gameObject.SetActive(false);

        // TODO: 씬 로드 시마다 필요
        InitManagers();
    }

    public void InitManagers()
    {
        GameManager.Init();
        UIManager.Init();
        SoundManager.Init(); 
        // DialogueManager.Init();
    }
}