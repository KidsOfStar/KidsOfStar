using UnityEngine;

public class ObstacleDespawner : MonoBehaviour
{
    public Transform[] tilemaps;
    public ObstaclesSpawner obstaclesSpawner;
    public float scrollSpeed = 7f;

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

        if (collision.CompareTag("Loopable"))
        {
            // 타일맵을 루프
        }
    }
}