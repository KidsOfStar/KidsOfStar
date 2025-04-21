using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgLooper : MonoBehaviour
{
    public ObstaclesSpawner obstaclesSpawner;
    public Vector3 obstacleLastPosition = Vector3.zero;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            Managers.Instance.PoolManager.Despawn(collision.gameObject);

            if (obstaclesSpawner != null)
            {
                obstaclesSpawner.OnObstacleDespawned();
            }
        }
    }
}