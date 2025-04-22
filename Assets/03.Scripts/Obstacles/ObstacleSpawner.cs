using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesSpawner : MonoBehaviour
{
    [Header("Settings")]
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
    private int currentWave = 1;      // 현재 Wave

    [Header("Dialogue Index")]
    [SerializeField] private int[] indexes;
    private int currentIndex;
    private readonly WaitForSeconds dialogEndTime = new(4f);

    public void StartSpawn()
    {
        foreach (GameObject prefab in obstaclePrefabs)
        {
            Managers.Instance.PoolManager.CreatePool(prefab, 10);
        }

        SpawnWave();
    }

    private void SpawnWave()
    {
        var mainCam = Managers.Instance.GameManager.MainCamera;
        float screenRight = mainCam.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        lastSpawnX = screenRight + spawnXOffset;

        currentWaveRemaining = waveObstacleCount;

        for (int i = 0; i < waveObstacleCount; i++)
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
        float spawnX = Random.Range(minSpacing, maxSpacing);
        lastSpawnX += spawnX;
        return new Vector3(lastSpawnX, fixedPosY, 0f);
    }

    private void SpawnNextObstacle()
    {
        ObstacleType chosenType = ChooseRandomTypeSpawner();
        string poolKey = GetPoolKey(chosenType);

        GameObject obj = Managers.Instance.PoolManager.Spawn(poolKey, Vector3.zero, Quaternion.identity);
        Obstacle obstacle = obj.GetComponent<Obstacle>();
        if (obstacle)
        {
            Vector3 spawnPos = GetSpawnPosition();
            obstacle.InitObstacle(spawnPos, chosenType);
        }
    }

    public void OnObstacleDespawned()
    {
        currentWaveRemaining--;

        // 한 웨이브가 끝났다면
        if (currentWaveRemaining > 0) return;

        if (currentWave >= 3)
        {
            Managers.Instance.DialogueManager.OnDialogEnd -= SpawnWave;
            Managers.Instance.SceneLoadManager.LoadScene(SceneType.Chapter2);
        }
        else
        {
            currentWave++;
                
            // 대사 끝났음 이벤트에 스폰 웨이브 구독
            Managers.Instance.DialogueManager.OnDialogEnd -= SpawnWave;
            Managers.Instance.DialogueManager.OnDialogEnd += SpawnWave;
                
            // 대사 출력
            Managers.Instance.DialogueManager.SetCurrentDialogData(indexes[currentIndex]);
            currentIndex++;

            StartCoroutine(OnDialogEnd());
        }
    }

    private IEnumerator OnDialogEnd()
    {
        yield return dialogEndTime;

        Managers.Instance.DialogueManager.OnClick?.Invoke();
    }
}