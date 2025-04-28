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

    public void StartLeafJump(Vector3 dropPosition, LayerMask groundMask, float jumpPower)
    {
        // 물리 초기화
        rb.velocity = Vector2.zero;
        rb.gravityScale = 1f;

        float power = rb.mass;
        rb.AddForce(dropPosition* power, ForceMode2D.Impulse);
    }
}
