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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isUsed) return;
        EditorLog.Log($"[Leaf] 충돌 감지됨: {collision.collider.name}");
        
        if ((collision.collider.CompareTag("Player") || collision.collider.CompareTag("Box")) && CheckBoundary(collision.transform))
        {
            EditorLog.Log("[Leaf] 점프 조건 만족, ActiveLeaf 시작!");
            StartCoroutine(ActiveLeaf(collision.gameObject));
        }
    }

    private bool CheckBoundary(Transform target)
    {
        float verticalTolerance = 0.3f;
        return target.position.y >= transform.position.y - verticalTolerance;
    }
    IEnumerator ActiveLeaf(GameObject target)
    {
        isUsed = true;

        var playerCtrl = target.GetComponent<PlayerController>();
        if (playerCtrl != null) playerCtrl.IsControllable = false;

        var rb = target.GetComponent<Rigidbody2D>();
        float originalGravity = 1f;
        if (rb != null)
        {
            originalGravity = rb.gravityScale;
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        DropLeaf();

        Vector3 start = target.transform.position;
        Vector3 end = dropPosition;

        float duration = 0.7f; // 총 이동 시간
        float elapsed = 0f;
        bool collDuringJump = false;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            Vector3 pos = Vector3.Lerp(start, end, t);
            pos.y += Mathf.Sin(t * Mathf.PI) * jumpHeight;
            // target.transform.position = pos;
            // RaycastHit2D hit = Physics2D.Raycast(target.transform.position, Vector2.down, 0.5f, obstacleMask);
            if (rb != null)
            {
                rb.MovePosition(pos);
            }
            else
            {
                target.transform.position = pos;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }
        if (collDuringJump)
        {
            Vector3 current = target.transform.position;
            Vector3 fallTarget = current + Vector3.down * 10f; 

            RaycastHit2D groundHit = Physics2D.Raycast(current, Vector2.down, 20f, obstacleMask);
            if (groundHit.collider != null)
            {
                fallTarget = new Vector3(current.x, groundHit.point.y, current.z);
            }

            while (Vector2.Distance(target.transform.position, fallTarget) > 0.05f)
            {
                //Test
                //target.transform.position = Vector2.MoveTowards(
                //    target.transform.position,
                //    fallTarget,
                //    moveSpeed * Time.deltaTime);
                Vector2 newPos = Vector2.MoveTowards(target.transform.position, fallTarget, moveSpeed * Time.deltaTime);
                if (rb != null)
                    rb.MovePosition(newPos);
                else
                    target.transform.position = newPos;

                yield return null;
            }
        }
        else
        {
            if (rb != null)
                rb.MovePosition(dropPosition);
            else
                target.transform.position = dropPosition;
            //target.transform.position = dropPosition;
        }

        if (rb != null)
            rb.gravityScale = originalGravity;

        if (playerCtrl != null)
            playerCtrl.IsControllable = true;
    }

    private void DropLeaf()
    {
        Rigidbody2D leafRb = gameObject.GetComponent<Rigidbody2D>();
        leafRb.gravityScale = 1f;
        Destroy(gameObject, 1f);
    }

    public void ResetLeaf(Vector2 originalPosition)
    {
        isUsed = false;
        transform.position = originalPosition;
        transform.rotation = Quaternion.identity;
        Rigidbody2D leafRb = gameObject.GetComponent<Rigidbody2D>();
        if (leafRb != null)
            Destroy(leafRb);
        gameObject.SetActive(true);
    }
}
