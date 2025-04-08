using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab; //PoolManager에 등록된 수초의 프리펩
    public int poolSize; // 폴에 생성할 장애물의 개수
    public float spawnInterval = 3f; //장애물 생성 간격
    public float obstacleDespawnTime = 1f; //장애물 Despawn까지의 시간
    private float spawnTimer;

    [Range(0f, 1f)] //생성되는 수초의 랜덤확률
    public float smallProbablility = 0.2f;
    public float middleProbablility = 0.35f;
    public float largeProbablility = 0.45f;

    public float minX = -5f;
    public float maxX = 10f;
    public float fixedY = -3f;

    private void Start()
    {
        Managers.Instance.PoolManager.CreatePool(obstaclePrefab, poolSize); //pool에서 obj 생성

        spawnTimer = spawnInterval;                                                                    
    }
    private void SpawnObstacle()
    {
        Vector3 spawnPos = new Vector3(Random.Range(minX,maxX),fixedY,0);
        GameObject obstacle = Managers.Instance.PoolManager.Spawn(obstaclePrefab.name, spawnPos, Quaternion.identity);

        StartCoroutine(DespawnAfterTime(obstacle, obstacleDespawnTime));

        float rand = Random.value;
        seaweedType selectedType;
        if (rand < smallProbablility)
        {
            selectedType = seaweedType.small;
        }
        else if (rand > smallProbablility + middleProbablility)
        {
            selectedType = seaweedType.medium;
        }
        else
        {
            selectedType = seaweedType.large;
        }
    }
    private IEnumerator DespawnAfterTime(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Managers.Instance.PoolManager.Despawn(obj);
    }

}
