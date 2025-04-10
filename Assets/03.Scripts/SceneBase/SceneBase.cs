using System;
using UnityEngine;

public class SceneBase : MonoBehaviour
{
    [Header("NPCs")]
    [SerializeField] private NPC[] Npcs;
    
    public Action onSceneLoadComplete;

    // 풀 생성, npc 넘겨주기 등 씬 초기화에 필요한 작업들을 담당
    // 씬이 로드될 때 호출되는 메서드

}
