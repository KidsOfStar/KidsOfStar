using System;
using UnityEngine;

// 풀 생성, npc 넘겨주기 등 씬 초기화에 필요한 작업들을 담당
public class SceneBase : MonoBehaviour
{
    [Header("NPCs")]
    [SerializeField] private NPC[] npcs;

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    
    protected virtual void Awake()
    {
        Managers.Instance.GameManager.SetCamera(mainCamera);
        Managers.Instance.OnSceneLoaded();
        Managers.Instance.DialogueManager.InitNPcs(npcs);
    }
    
    protected void CreateSelectionUI()
    {
        var selectObj = Managers.Instance.ResourceManager.Load<GameObject>(Define.UIPath + typeof(UISelectButton));
        Managers.Instance.PoolManager.CreatePool(selectObj, 5);
    }
}
