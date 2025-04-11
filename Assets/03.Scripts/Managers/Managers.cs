using UnityEngine;

public class Managers : Singleton<Managers>
{
    [field: SerializeField] public SceneLoadManager SceneLoadManager { get; private set; }
    
    public ResourceManager ResourceManager { get; private set; }
    public DataManager DataManager { get; private set; }
    public PoolManager PoolManager { get; private set; }
    public UIManager UIManager { get; private set; }
    public SoundManager SoundManager { get; private set; }
    public DialogueManager DialogueManager { get; private set; }
    public GameManager GameManager { get; private set; }
    
    public CutSceneManager cutSceneManager { get; private set; }

    protected override void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        base.Awake();
        ResourceManager = new ResourceManager();
        DataManager = new DataManager();
        PoolManager = new PoolManager();
        GameManager = new GameManager();
		UIManager = new UIManager();
        SoundManager = new SoundManager();
        DialogueManager = new DialogueManager();
        cutSceneManager = new CutSceneManager();
    }

    public void OnSceneLoaded()
    {
        UIManager.OnSceneLoaded();
        SoundManager.OnSceneLoaded();
        DialogueManager.OnSceneLoaded();
    }
    
    public void OnSceneUnloaded()
    {
        SoundManager.OnSceneUnloaded();
        DialogueManager.OnSceneUnloaded();
    }
}