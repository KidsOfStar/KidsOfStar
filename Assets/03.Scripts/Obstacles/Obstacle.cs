using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Obstacle : ScrollObject
{
    private float mediumYOffset = 0.1f;
    private float largeYOffset = 0.2f;
    private float fixedPosY = -2.7f;

    public void InitObstacle(Vector3 spawnPosition, ObstacleType chosenType)
    {
        if(chosenType == ObstacleType.Stone)
        {
            fixedPosY = -3f;
        }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            // Managers.Instance.GameManager.GameOver();
        }
    }
}
