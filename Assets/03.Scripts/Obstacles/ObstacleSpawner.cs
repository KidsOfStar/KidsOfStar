//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ObstaclesSpawner : MonoBehaviour
//{
//    public GameObject[] obstaclePrefab; //PoolManager에 등록된 수초의 프리펩
//    public int poolSize; // 폴에 생성할 장애물의 개수
//    public float spawnInterval = 3f; //장애물 생성 간격
//    public float obstacleDespawnTime = 1f; //장애물 Despawn까지의 시간
//    private float spawnTimer;

//    [Range(0f, 1f)] //생성되는 수초의 랜덤확률
//    public float smallProbablility = 0.2f;
//    [Range(0f, 1f)]
//    public float middleProbablility = 0.35f;
//    [Range(0f, 1f)]
//    public float largeProbablility = 0.45f;

//    public float minX = -5f;
//    public float maxX = 10f;
//    public float fixedY = -3f;

//    private void Start()
//    {
//        Managers.Instance.PoolManager.CreatePool(obstaclePrefab[0], poolSize); //pool에서 obj 생성
//        Managers.Instance.PoolManager.CreatePool(obstaclePrefab[1], poolSize);
//        Managers.Instance.PoolManager.CreatePool(obstaclePrefab[2], poolSize);

//        spawnTimer = spawnInterval;
//    }
//    private void Update()
//    {
//        spawnTimer -= Time.deltaTime;
//        if(spawnTimer<=0f)
//        {
//            SpawnObstacle();
//            spawnTimer = spawnInterval;
//        }
//    }
//    private void SpawnObstacle()
//    {
//        Vector3 spawnPos = new Vector3(Random.Range(minX,maxX),fixedY,0);
//        //GameObject obstacle = Managers.Instance.PoolManager.Spawn(obstaclePrefab.name, spawnPos, Quaternion.identity);

//        float rand = Random.value;
//        seaweedType selectedType;
//        if (rand < smallProbablility)
//        {
//            selectedType = seaweedType.small;
//        }
//        else if (rand > smallProbablility + middleProbablility)
//        {
//            selectedType = seaweedType.medium;
//        }
//        else
//        {
//            selectedType = seaweedType.large;
//        }
//        Obstacle obstacleComponent = obstacle.GetComponent<Obstacle>();
//        if (obstacleComponent != null)
//        {
//            obstacleComponent.InitializeObstacle(selectedType);
//        }

//        StartCoroutine(DespawnAfterTime(obstacle, obstacleDespawnTime));

//    }
//    private IEnumerator DespawnAfterTime(GameObject obj, float delay)
//    {
//        yield return new WaitForSeconds(delay);
//        Managers.Instance.PoolManager.Despawn(obj);
//    }

//    //private void OnTriggerEnter2D(Collider2D collision)
//    //{
//    //    Obstacle obstacle = collision.GetComponent<Obstacle>();
//    //    if (obstacle != null)
//    //    {
//    //        Managers.Instance.PoolManager.Despawn(gameObject);

//    //        SpawnObstacle();
//    //    }
//    //}
//}
