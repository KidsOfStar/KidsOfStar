using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPointer : MonoBehaviour
{
    [Header("SpawnPoints 부모 컨테이너 할당")]
    [SerializeField] private Transform spawnPointsRoot;

    private Dictionary<CutSceneType, Transform> map;
    private Transform player;

    private void Awake()
    {
        map = new Dictionary<CutSceneType, Transform>();

        // 부모 포함해서 전체 자식 트랜스폼을 모두 가져온 뒤
        var all = spawnPointsRoot.GetComponentsInChildren<Transform>();

        foreach (var child in all)
        {
            // 루트 자신은 스킵
            if (child == spawnPointsRoot)
                continue;

            // 이름이 enum 이름과 일치하면 매핑
            if (Enum.TryParse(child.name, out CutSceneType type))
            {
                if (!map.ContainsKey(type))
                    map[type] = child;
                else
            }
            else
            {
                // 맞지 않는 이름은 그냥 넘어가거나, 경고 출력
            }
        }
    }


    private void Start()
    {
        player = Managers.Instance.CutSceneManager.PlayerTransform;
        Managers.Instance.CutSceneManager.OnCutSceneEnd += HandleCutSceneEnd;
    }

    private void OnDisable()
    {
        Managers.Instance.CutSceneManager.OnCutSceneEnd -= HandleCutSceneEnd;
    }

    private void HandleCutSceneEnd()
    {
        var name = Managers.Instance.CutSceneManager.CurrentCutSceneName;
        if (Enum.TryParse(name, out CutSceneType cutScene)
            && map.TryGetValue(cutScene, out var target))
        {
            player.position = target.position;
            player.rotation = target.rotation;
        }
    }
}
