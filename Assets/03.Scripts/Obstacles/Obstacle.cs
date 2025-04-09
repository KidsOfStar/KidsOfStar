using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public enum seaweedType
{
    small,
    medium,
    large,
}
public class Obstacle : MonoBehaviour
{
    private Animator ani;
    private Vector3 originalLocalPosition;

    private Vector3 smallYOffset = new Vector3(0, -0.25f, 0);
    private Vector3 mediumYOffset = new Vector3(0, -0.1f, 0);

    private void Awake()
    {
        originalLocalPosition = transform.localPosition;
    }

    public void ResetObstacle()
    {
        transform.localPosition = originalLocalPosition;
    }
    public void InitializeObstacle(seaweedType type)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        ResetObstacle();

        switch (type)
        {
            case seaweedType.small:
                transform.localPosition += smallYOffset;
                //ani.Play(); 작은 수초 애니메이션 
                break;
            case seaweedType.medium:
                transform.localPosition += mediumYOffset;
                //ani.Play();
                break;
            case seaweedType.large:
                //ani.Play();
                break;
                
        }
    }


}
