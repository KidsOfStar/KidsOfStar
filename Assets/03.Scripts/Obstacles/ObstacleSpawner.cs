using System.Collections.Generic;
using UnityEngine;

public class ObstaclesSpawner : MonoBehaviour
{
    public float spawnXOffset = 10f;

    private float fixedPosY = -2.7f;

    public float minSpacing = 2f;
    public float maxSpacing = 5f;

    private float stoneProbability = 0.3f;
    private float smallSeaweedProbability = 0.2f;
    private float mediumSeaweedProbability = 0.35f;

    public List<GameObject> obstaclePrefabs;
    private float lastSpawnX;

    [Header("Wave")]
    public int waveObstacleCount = 8; //Wave에 생성할 장애물의 갯수
    private int currentWaveRemaining; //Wave에서 소멸되지 않은 장애물의 갯수 
    private int currentWave = 1; // 현재 Wave
    private void Start()
    {
        foreach(GameObject prefab in obstaclePrefabs)
        {
            Managers.Instance.PoolManager.CreatePool(prefab, 10);
        }
        SpawnWave();
    }

    private void SpawnWave()
    {
        float screenRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        lastSpawnX = screenRight + spawnXOffset;

        currentWaveRemaining = waveObstacleCount;

        for(int i =0; i<waveObstacleCount; i++)
        {
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

    private Vector3 GetSpawnPosition()
    {
        float spawnX = Random.Range(minSpacing,maxSpacing);
        lastSpawnX += spawnX;
        return new Vector3(lastSpawnX, fixedPosY, 0f);
    }

    public void SpawnNextObstacle()
    {
        ObstacleType chosenType = ChooseRandomTypeSpawner();
        string poolKey = GetPoolKey(chosenType);

        GameObject obj = Managers.Instance.PoolManager.Spawn(poolKey, Vector3.zero, Quaternion.identity);
        Obstacle obstacle = obj.GetComponent<Obstacle>();
        if (obstacle != null)
        {
            Vector3 spawnPos = GetSpawnPosition();
            obstacle.InitObstacle(spawnPos, chosenType);
        }
    }
    public void OnObstacleDespawned()
    {
        currentWaveRemaining--;

        if (currentWaveRemaining <= 0)
        {
            if (currentWave >= 3)
            {
                // Managers.Instance.SceneManager.LoadScene(// 해당 다음 NextScene)
            }
            else
            {
                currentWave++;
                SpawnWave();
            }
        }
    }
}
