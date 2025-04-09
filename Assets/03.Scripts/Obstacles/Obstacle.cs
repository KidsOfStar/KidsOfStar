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
    private float mediumYOffset = 0.1f;
    private float largeYOffset = 0.2f;
    private float fixedPosY = -3.4f;

    public float scrollSpeed = 2f;
    public void InitObstacle(Vector3 spawnPosition, ObstacleType chosenType)
    {
        if (chosenType == ObstacleType.MediumSeaweed)
        {
            fixedPosY += mediumYOffset;
        }
        else if (chosenType == ObstacleType.LargeSeaweed)
        {
            fixedPosY += largeYOffset;
        }
        Vector3 pos = new Vector3(spawnPosition.x, fixedPosY, 0f);
        transform.position = pos;
        // SetupAnimation();  
    }
    private void Update()
    {
        transform.position += Vector3.left * scrollSpeed * Time.deltaTime;
    }
}
