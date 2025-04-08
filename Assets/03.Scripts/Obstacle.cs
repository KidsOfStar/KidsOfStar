using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum seaweedType
{
    small,
    medium,
    large,
}
public class Obstacle : MonoBehaviour
{
    private Animator ani;

    public Sprite smallSprite;
    public Sprite mediumSprite;
    public Sprite largeSprite;

    private Vector3 smallYOffset = new Vector3(0, -0.25f, 0);
    private Vector3 mediumYOffset = new Vector3(0, -0.1f, 0);
    public void InitializeObstacle(seaweedType type)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        switch(type)
        {
            case seaweedType.small:
                sr.sprite = smallSprite;
                transform.localPosition += smallYOffset;
                //ani.Play(); 작은 수초 애니메이션 
                break;
            case seaweedType.medium:
                sr.sprite = mediumSprite;
                transform.localPosition += mediumYOffset;
                //ani.Play();
                break;
            case seaweedType.large:
                sr.sprite = largeSprite;
                //ani.Play();
                break;
                
        }
    }


}
