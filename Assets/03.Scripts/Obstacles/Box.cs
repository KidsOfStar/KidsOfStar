using System.Collections;
using UnityEngine;
public interface IWeightable
{
    float GetWeight();
    Rigidbody2D GetRigidbody2D();
}

public class Box : MonoBehaviour, IWeightable, ILeafJumpable
{
    public float boxWeight = 2f;

    [Tooltip("Player 레이어와의 충돌을 무시할 시간")]
    public float ignoreDuration = 0.5f;

    private Rigidbody2D rb;
    private int boxLayer;
    private int playerLayer;
    public Vector3 boxBasePos;

    void Awake()
    {
        rb= GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.gravityScale = 1f;
        boxLayer = gameObject.layer;
        playerLayer = LayerMask.NameToLayer("Player");
    }

    // 박스의 Weight를 가져오는 메서드로 IWeightable로 구현
    public float GetWeight()
    {
        return boxWeight;
    }
    
    public Rigidbody2D GetRigidbody2D()
    {
        return rb;
    }

    public void StartLeafJump(Vector2 dropPosition, float jumpPower)
    {
        // 레이어간 충돌을 무시하는 코루틴 시작
        StartCoroutine(TemporaryIgnorePlayer(ignoreDuration));

        // 물리 초기화
        rb.gravityScale = 1f;
        
        Vector2 impulse = dropPosition * jumpPower;

        rb.AddForce(impulse, ForceMode2D.Impulse);
    }

    // 특정 레이어 간의 충돌을 무시했다가 돌리는 코루틴
    private IEnumerator TemporaryIgnorePlayer(float duration)
    {
        // 박스, 플레이어 레이어간 충돌 판정을 무시
        Physics2D.IgnoreLayerCollision(boxLayer, playerLayer, true);

        // duration만큼만 무시
        yield return new WaitForSeconds(duration);

        // 박스, 플레이어 레이어간 충돌 판정 다시 허용
        Physics2D.IgnoreLayerCollision(boxLayer, playerLayer, false);
    }

    public void ResetPosition()
    {
        this.gameObject.transform.position = boxBasePos;
    }
}
