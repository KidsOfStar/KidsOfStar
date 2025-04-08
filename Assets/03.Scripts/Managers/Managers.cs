using UnityEngine;

public class Managers : Singleton<Managers>
{
    [field: SerializeField] public SceneLoadManager SceneLoadManager { get; private set; }
    
    public ResourceManager ResourceManager { get; private set; }
    public DataManager DataManager { get; private set; }
    public PoolManager PoolManager { get; private set; }
    public SoundManager SoundManager { get; private set; }
    public DialogueManager DialogueManager { get; private set; }
    public GameManager GameManager { get; private set; }
    public UIManager UIManager { get; private set; }

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

       InitManagers();
    }

    public void InitManagers()
    {
        DataManager.Init();
        GameManager.Init();
        SoundManager.Init();
        DialogueManager.Init();
    }
}
