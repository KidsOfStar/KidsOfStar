using System;
using UnityEngine;
using UnityEngine.Events;

// 풀 생성, npc 넘겨주기 등 씬 초기화에 필요한 작업들을 담당
public class SceneBase : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private GameObject playerPrefab; // TODO: 리소스 로드 할지?
    [SerializeField] private Transform playerSpawnPosition;
    
    [Header("NPCs")]
    [SerializeField] private NPC[] speakers;

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    
    [Header("Events")]
    [SerializeField] protected UnityEvent onLoadComplete;
    
    // TODO: 각 씬 별로 플레이어가 자유상호작용 때 말하는 부분이 있다면 플레이어도 스폰 후 speaker로 등록해야함
    protected virtual void Awake()
    {
        Managers.Instance.GameManager.SetCamera(mainCamera);
        Managers.Instance.OnSceneLoaded();
        Managers.Instance.DialogueManager.InitSceneNPcs(speakers);
        PlayerUI();
    }

    protected void PlayerUI()
    {
        Managers.Instance.UIManager.Show<PlayerBtn>();
        Managers.Instance.UIManager.Show<UIJoystick>();
    }
    
    protected void SpawnPlayer()
    {
        var gameManager = Managers.Instance.GameManager;
        Vector3 playerPosition = gameManager.IsNewGame ? playerSpawnPosition.position : gameManager.PlayerPosition;
            
        GameObject player = Instantiate(playerPrefab, playerPosition, Quaternion.identity);
        Managers.Instance.GameManager.SetPlayer(player.GetComponent<Player>());
    }
}
