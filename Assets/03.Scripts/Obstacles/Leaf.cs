using System.Collections;
using UnityEngine;

public class Leaf : MonoBehaviour
{
    [Header("Jump Settings")]
    public Vector3 dropPosition;
    public float moveSpeed = 3f;
    public float jumpHeight = 10f;

    [Header("Respawn Settings")]
    [Tooltip("Leaf가 떨어지고 다시 스폰되기 전까지 대기할 시간 (초)")]
    [SerializeField] private float respawnDelay = 5f;
    [Tooltip("Leaf가 떨어지고 사라지기까지의 시간 (초)")]
    [SerializeField] private float fallDuration = 1f;

    [Header("충돌 감지 레이어")]
    public LayerMask obstacleMask;

    private bool isUsed = false;
    private Rigidbody2D leafRb;
    private SpriteRenderer sr;
    private Collider2D leafCollider;
    private Vector2 originalPosition;
    private Quaternion originalRotation;
    void Start()
    {
        leafRb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        leafCollider = GetComponent<Collider2D>();
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        leafRb.gravityScale = 0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isUsed) return;
        if ((collision.collider.CompareTag("Player") || collision.collider.CompareTag("Box")) && CheckBoundary(collision.transform))
        {
            if (collision.gameObject.TryGetComponent<ILeafJumpable>(out var jumpable))
            {
                // 점프 트리거 호출
                jumpable.StartLeafJump(dropPosition, obstacleMask, moveSpeed, jumpHeight);
                isUsed = true;
                // 떨어지고 재생성 코루틴 시작
                StartCoroutine(DropAndRespawn());
            }
        }
    }

    private bool CheckBoundary(Transform target)
    {
        float verticalTolerance = 0.3f;
        return target.position.y >= transform.position.y - verticalTolerance;
    }

    private IEnumerator DropAndRespawn()
    {
        // 중력 활성화하여 떨어지게 함
        leafRb.gravityScale = 1f;

        // 사라지기 전까지 대기
        yield return new WaitForSeconds(fallDuration);

        // 비활성화 대신 렌더러와 콜라이더만 끄기
        leafRb.gravityScale = 0f;
        leafRb.velocity = Vector2.zero;
        sr.enabled = false;
        leafCollider.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        // 원위치로 리셋
        ResetLeaf();
    }

    public void ResetLeaf() 
    {
        isUsed = false;
        transform.position = originalPosition;
        transform.rotation = Quaternion.identity;
        sr.enabled = true;
        leafCollider.enabled = true;
        leafRb.gravityScale = 0f;
    }
}
