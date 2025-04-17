using System.Collections;
using UnityEngine;

public class Leaf : MonoBehaviour
{
    public Vector3 dropPosition;
    public float moveSpeed = 3f;
    public float jumpHeight = 10f;

    private bool isUsed = false;

    [Header("충돌 감지 레이어")]
    public LayerMask obstacleMask;

    private Rigidbody2D leafRb;
    void Start()
    {
        leafRb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isUsed) return;
        EditorLog.Log($"[Leaf] 충돌 감지됨: {collision.collider.name}");

        if ((collision.collider.CompareTag("Player") || collision.collider.CompareTag("Box")) && CheckBoundary(collision.transform))
        {
            if (collision.gameObject.TryGetComponent<ILeafJumpable>(out var jumpable))
            {
                jumpable.StartLeafJump(dropPosition, obstacleMask, moveSpeed, jumpHeight);
                isUsed = true;
                DropLeaf();
            }
        }
    }

    private bool CheckBoundary(Transform target)
    {
        float verticalTolerance = 0.3f;
        return target.position.y >= transform.position.y - verticalTolerance;
    }

    private void DropLeaf()
    {
        leafRb.gravityScale = 1f;
        Destroy(gameObject, 2f);
    }

    public void ResetLeaf(Vector2 originalPosition) //추후 사용할 예정인건지
    {
        isUsed = false;
        transform.position = originalPosition;
        transform.rotation = Quaternion.identity;
        if (leafRb != null)
            Destroy(leafRb);
        gameObject.SetActive(true);
    }
}
