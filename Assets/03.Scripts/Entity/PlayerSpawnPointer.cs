using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPointer : MonoBehaviour
{

    [Header("SpawnPoints 부모 컨테이너 할당")]
    [SerializeField] private Transform spawnPointsRoot;

    private Dictionary<CutSceneType, Transform> map;
    private Transform player;

    //[Header("Inspector에 할당해야 할 것")]
    //[Tooltip("컷씬이 끝난 뒤 이동시킬 플레이어 Transform")]
    //[SerializeField] private Transform player;
    //[Tooltip("컷씬 종료 후 플레이어를 보낼 위치 Transform")]
    //[SerializeField] private List<Transform> spawnPoints;

    //[Tooltip("컷신 종류")]
    //[SerializeField] private List<string> currentCutSceneNames;

    private void Awake()
    {
        map = new Dictionary<CutSceneType, Transform>();

        // 1) 부모 포함해서 전체 자식 트랜스폼을 모두 가져온 뒤
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
                    Debug.LogWarning($"[PlayerSpawnPointer] 중복된 자식 이름: {child.name}");
            }
            else
            {
                // 맞지 않는 이름은 그냥 넘어가거나, 경고 출력
                Debug.LogWarning($"[PlayerSpawnPointer] '{child.name}'은 CutSceneType이 아닙니다.");
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

    //private void HandleCutSceneEnd()
    //{
    //    // 현재 재생된 컷씬 타입이 우리가 지정한 타입이 아니면 무시
    //    string current = Managers.Instance.CutSceneManager.CurrentCutSceneName;
    //    int idx = currentCutSceneNames.IndexOf(current);
    //    if (idx < 0) return;

    //    if (spawnPoints == null || idx >= spawnPoints.Count) return;

    //    Transform targetPoint = spawnPoints[idx];

    //    player.position = targetPoint.position;
    //    player.rotation = targetPoint.rotation;

    //    // 한 번만 처리하고 싶다면 여기서 해제
    //    Managers.Instance.CutSceneManager.OnCutSceneEnd -= HandleCutSceneEnd;
    //}
    private void HandleCutSceneEnd()
    {
        var name = Managers.Instance.CutSceneManager.CurrentCutSceneName;
        if (Enum.TryParse(name, out CutSceneType cutScene)
            && map.TryGetValue(cutScene, out var target))
        {
            player.position = target.position;
            player.rotation = target.rotation;
        }
        // 한 번만 처리하고 싶으면 아래 줄 주석 해제
        // Managers.Instance.CutSceneManager.OnCutSceneEnd -= HandleCutSceneEnd;
    }
}
