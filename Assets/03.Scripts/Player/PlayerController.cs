using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigid;
    private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask groundLayer;

    // 플레이어 캐릭터 이동 속도
    [SerializeField] private float moveSpeed;
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }
    // 플레이어 캐릭터 점프력
    [SerializeField] private float jumpForce;
    public float JumpForce
    {
        get { return jumpForce; }
        set { jumpForce = value; }
    }
    // 플레이어 캐릭터가 사다리에 닿은 상태일 때 true
    private bool touchLadder = false;
    public bool TouchLadder
    {
        get { return touchLadder; }
        set { touchLadder = value; }
    }
    // 플레이어 캐릭터가 땅에 닿으면 true
    private bool isGround;
    // 플레이어 캐릭터 조작 가능하면 true
    private bool isControllable = true;
    public bool IsControllable
    {
        get { return isControllable; }
        set { isControllable = value; }
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        GroundCheck();
    }

    void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down,
            0.02f, groundLayer);
        isGround = hit.collider != null ? true : false;
    }

    void OnMove(InputValue value)
    {
        if (!isControllable) return;

        Vector2 dir = value.Get<Vector2>();
        if(dir == Vector2.zero)
        {
            rigid.velocity = Vector2.zero;
            return;
        }

        if(touchLadder)
        {

        }
        else
        {
            if(dir != Vector2.up && dir != Vector2.down)
            {
                rigid.velocity = dir * moveSpeed;
            }
        }
    }

    void OnJump(InputValue value)
    {
        if(isGround)
        {
            rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
