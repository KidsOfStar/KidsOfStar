using Cinemachine;
using UnityEngine;

public class Chapter01 : SceneBase
{
    [SerializeField] private CinemachineVirtualCamera wideCamera;
    
    protected override void Awake()
    {
        base.Awake();
        // test
        
        
        // SpawnPlayer();
        // SceneLoadManager의 완료 상태를 설정
        Managers.Instance.GameManager.SetVirtualCamera(wideCamera);
        Managers.Instance.SceneLoadManager.IsSceneLoadComplete = true;
    }
}
