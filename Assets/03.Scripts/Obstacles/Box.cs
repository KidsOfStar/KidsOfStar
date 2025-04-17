using System.Collections;
using UnityEngine;

public interface ILeafJumpable
{
    void StartLeafJump(Vector3 dropPosition, LayerMask groundMask, float moveSpeed, float jumpHeight);
}
public class Box : MonoBehaviour, IWeightable, ILeafJumpable
{
    public float boxWeight = 2f;

    private Rigidbody2D rb;
    private Collider2D col;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.gravityScale = 1f;
        rb.mass = boxWeight;
    }

    public float GetWeight()
    {
        return boxWeight;
    }

    public void StartLeafJump(Vector3 dropPosition, LayerMask groundMask, float moveSpeed, float jumpHeight)
    {
        StartCoroutine(LeafJumpRoutine(dropPosition, groundMask, moveSpeed, jumpHeight));
    }
    private IEnumerator LeafJumpRoutine(Vector3 dropPosition, LayerMask groundMask, float moveSpeed, float jumpHeight)
    {
        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        col.enabled = false;

        Vector3 start = transform.position;
        Vector3 end = dropPosition;
        float duration = 0.7f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            Vector3 pos = Vector3.Lerp(start, end, t);
            pos.y += Mathf.Sin(t * Mathf.PI) * jumpHeight;

            rb.MovePosition(pos);

            // 바닥 감지
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(0.6f, 0.3f), 0f, Vector2.down, 0.1f, groundMask);
            if (hit.collider != null)
            {
                Debug.Log("[Box] 점프 중 바닥 감지 → 즉시 착지");
                break;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 바닥 착지 지점 계산
        Vector3 fallTarget = transform.position;
        RaycastHit2D groundHit = Physics2D.BoxCast(fallTarget, new Vector2(0.6f, 0.3f), 0f, Vector2.down, 2f, groundMask);
        if (groundHit.collider != null)
            fallTarget = new Vector3(transform.position.x, groundHit.point.y, transform.position.z);

        while (Vector2.Distance(transform.position, fallTarget) > 0.05f)
        {
            Vector2 newPos = Vector2.MoveTowards(transform.position, fallTarget, moveSpeed * Time.deltaTime);
            rb.MovePosition(newPos);
            yield return null;
        }

        rb.gravityScale = 1f;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.drag = 4f;
        col.enabled = true;

        yield return new WaitForSeconds(0.3f);
        rb.drag = 0f;
    }
}
