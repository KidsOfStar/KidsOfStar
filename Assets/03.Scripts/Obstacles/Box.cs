using System.Collections;
using UnityEngine;
public interface IWeightable
{
    float GetWeight();
}
public class Box : MonoBehaviour, IWeightable, ILeafJumpable
{
    public float boxWeight = 2f;

    [Tooltip("Player 레이어와의 충돌을 무시할 시간")]
    public float ignoreDuration = 0.5f;

    private Rigidbody2D rb;
    private int boxLayer;
    private int playerLayer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.gravityScale = 1f;
        rb.mass = boxWeight;
        boxLayer = gameObject.layer;
        playerLayer = LayerMask.NameToLayer("Player");
    }

    public float GetWeight()
    {
        return boxWeight;
    }

    public void StartLeafJump(Vector2 dropPosition, float jumpPower)
    {
        StartCoroutine(TemporaryIgnorePlayer(ignoreDuration));

        // 물리 초기화
        rb.velocity = Vector2.zero;
        rb.gravityScale = 1f;
        
        Vector2 impulse = dropPosition * jumpPower * rb.mass;

        rb.AddForce(impulse, ForceMode2D.Impulse);
    }

    private IEnumerator TemporaryIgnorePlayer(float duration)
    {
        Physics2D.IgnoreLayerCollision(boxLayer, playerLayer, true);

        yield return new WaitForSeconds(duration);

        Physics2D.IgnoreLayerCollision(boxLayer, playerLayer, false);
    }
}
