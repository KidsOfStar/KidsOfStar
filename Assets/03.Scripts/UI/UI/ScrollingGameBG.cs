using UnityEngine;

public class ScrollingGameBG : MonoBehaviour
{
    [Tooltip("스크롤 속도")]
    public float scrollSpeed;

    private Vector3 startPos;
    private float spriteWidth;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Time.time * scrollSpeed;
        float offset = Mathf.Repeat(distance, spriteWidth);
        float newX = startPos.x - offset;

        transform.position = new Vector3(newX, startPos.y, startPos.z);
    }
}
