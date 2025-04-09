using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum ObstacleType
{
    SmallSeaweed,
    MediumSeaweed,
    LargeSeaweed,
    Stone
}
public class Obstacle : MonoBehaviour
{
    public float minX = -5f;
    public float maxX = 7f;
    public float fixedPosY = -3.15f;

    public Vector3 SetRandomPlace()
    {
        float randomX = Random.Range(minX, maxX);
        Vector3 newPos = new Vector3(randomX, fixedPosY, 0f);
        transform.position = newPos;
        return newPos;
    }

    public void InitObstacle(Vector3 spawnPosition, ObstacleType chosenType)
    {
        Vector3 pos = new Vector3(spawnPosition.x, -3.15f, 0f);
        transform.position = pos;
        // SetupAnimation();  
    }
}
