using System.Collections;
using UnityEngine;
public interface IWeightable
{
    float GetWeight();
}

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
        // 물리상태 비활성화
        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        col.enabled = false;

        Vector3 start = transform.position;
        Vector3 end = dropPosition;

        // 바닥 착지 지점 계산
        Vector3 fallTarget = dropPosition;
        fallTarget.y += col.bounds.extents.y;
        rb.MovePosition(fallTarget);

        // 물리상태 초기화
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.gravityScale = 1f;
        col.enabled = true;

        yield return new WaitForSeconds(0.3f);
        // Test
        //rb.drag = 0f;
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
