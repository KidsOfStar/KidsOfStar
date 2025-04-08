using UnityEngine;

public class Managers : Singleton<Managers>
{
    [field: SerializeField]public ResourceManager ResourceManager { get; private set; }
    [field: SerializeField]public DataManager DataManager { get; private set; }
    [field: SerializeField]public PoolManager PoolManager { get; private set; }
    [field: SerializeField]public SoundManager SoundManager { get; private set; }
    [field: SerializeField]public GameManager GameManager { get; private set; }
}
