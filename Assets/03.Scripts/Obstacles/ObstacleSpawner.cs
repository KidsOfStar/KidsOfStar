using System.Collections.Generic;
using UnityEngine;

public class ObstaclesSpawner : MonoBehaviour
{
    private float stoneProbability = 0.3f;
    private float smallSeaweedProbability = 0.2f;
    private float mediumSeaweedProbability = 0.35f;
    public float spawnXOffset = 3f;
    private float lastSpawnX;

    public List<GameObject> obstaclePrefabs;

    private void Start()
    {
        foreach(GameObject prefab in obstaclePrefabs)
        {
            Managers.Instance.PoolManager.CreatePool(prefab, 4);
            SpawnNextObstacle();
        }
    }

    private string GetPoolKey(ObstacleType type)
    {
        switch (type)
        {
            case ObstacleType.SmallSeaweed:
                return "SmallSeaweed";
            case ObstacleType.MediumSeaweed:
                return "MediumSeaweed";
            case ObstacleType.LargeSeaweed:
                return "LargeSeaweed";
            case ObstacleType.Stone:
                return "Stone";
            default:
                return "";
        }
    }
    private ObstacleType ChooseRandomTypeSpawner()
    {
        float rand = Random.value;
        if (rand < stoneProbability)
        {
            return ObstacleType.Stone;
        }
        float seaweedRand = Random.value;
        if (seaweedRand < smallSeaweedProbability)
        {
            return ObstacleType.SmallSeaweed;
        }
        else if (seaweedRand < smallSeaweedProbability + mediumSeaweedProbability)
        {
            return ObstacleType.MediumSeaweed;
        }
        else
        {
            return ObstacleType.LargeSeaweed;
        }
    }

    public void SpawnNextObstacle()
    {

        ObstacleType chosenType = ChooseRandomTypeSpawner();
        string poolKey = GetPoolKey(chosenType);

        // PoolManager를 통해 결정된 풀 키의 장애물을 스폰합니다.
        GameObject obj = Managers.Instance.PoolManager.Spawn(poolKey, Vector3.zero, Quaternion.identity);
        Obstacle obstacle = obj.GetComponent<Obstacle>();
        if (obstacle != null)
        {
            Vector3 spawnPos = obstacle.SetRandomPlace();
            obstacle.InitObstacle(spawnPos, chosenType);
        }
    }
}
