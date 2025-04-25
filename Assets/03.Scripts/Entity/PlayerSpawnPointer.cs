using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPointer : MonoBehaviour
{
    [Header("Inspector에 할당해야 할 것")]
    [Tooltip("컷씬이 끝난 뒤 이동시킬 플레이어 Transform")]
    [SerializeField] private Transform player;
    [Tooltip("컷씬 종료 후 플레이어를 보낼 위치 Transform")]
    [SerializeField] private List<Transform> spawnPoints;

    [Tooltip("컷신 종류")]
    [SerializeField] private List<string> currentCutSceneNames;

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
        // 현재 재생된 컷씬 타입이 우리가 지정한 타입이 아니면 무시
        string current = Managers.Instance.CutSceneManager.CurrentCutSceneName;
        int idx = currentCutSceneNames.IndexOf(current);
        if (idx < 0) return;

        if (spawnPoints == null || idx >= spawnPoints.Count) return;

        Transform targetPoint = spawnPoints[idx];

        player.position = targetPoint.position;
        player.rotation = targetPoint.rotation;

        // 한 번만 처리하고 싶다면 여기서 해제
        Managers.Instance.CutSceneManager.OnCutSceneEnd -= HandleCutSceneEnd;
    }
}
