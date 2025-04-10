using System;
using UnityEngine;

// 풀 생성, npc 넘겨주기 등 씬 초기화에 필요한 작업들을 담당
public class SceneBase : MonoBehaviour
{
    [Header("NPCs")]
    [SerializeField] private NPC[] npcs;
    
    public Action onSceneLoadComplete;

    protected virtual void Awake()
    {
        Managers.Instance.OnSceneLoaded();
        Managers.Instance.DialogueManager.InitNPcs(npcs);
        
        // 씬 로드 완료
        onSceneLoadComplete?.Invoke();
    }

    private void OnDestroy()
    {
        Managers.Instance.OnSceneUnloaded();
    }
}
