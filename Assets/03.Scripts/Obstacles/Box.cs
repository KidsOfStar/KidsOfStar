using System.Collections;
using UnityEngine;
public interface IWeightable
{
    float GetWeight();
}
public class Box : MonoBehaviour, IWeightable, ILeafJumpable
{
    public float boxWeight = 2f;
    [Tooltip("Leaf에서 전달받을 점프 Power")]
    public float jumpPower;

    private Rigidbody2D rb;
    private Collider2D col;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.gravityScale = 1f;
        rb.mass = boxWeight;
    }

    public float GetWeight()
    {
        return boxWeight;
    }

    public void StartLeafJump(Vector2 dropPosition, float jumpPower)
    {
        StartCoroutine(TemporaryTrigger(0.5f));

        // 물리 초기화
        rb.velocity = Vector2.zero;
        rb.gravityScale = 1f;
        
        Vector2 dir = (dropPosition - rb.position).normalized;
        Vector2 impulse = dir * jumpPower * rb.mass;

        rb.AddForce(impulse, ForceMode2D.Impulse);
    }

    private IEnumerator TemporaryTrigger(float duration)
    {
        col.isTrigger = true;
        yield return new WaitForSeconds(duration);
        col.isTrigger = false;
    }
}
