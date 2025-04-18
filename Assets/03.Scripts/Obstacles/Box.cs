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

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 바닥 착지 지점 계산
        Vector3 fallTarget = dropPosition;
        fallTarget.y += col.bounds.extents.y;

        // 부드러운 낙하
        while (Vector2.Distance(transform.position, fallTarget) > 0.05f)
        {
            Vector2 newPos = Vector2.MoveTowards(transform.position, fallTarget, moveSpeed * Time.deltaTime);
            rb.MovePosition(newPos);
            yield return null;
        }

        // 딱 고정
        rb.MovePosition(fallTarget);
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.gravityScale = 1f;
        col.enabled = true;

        yield return new WaitForSeconds(0.3f);
        rb.drag = 0f;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
            EditorLog.Log(rb.constraints);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
