using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("공통")]
    [SerializeField] private LayerMask groundLayer;
    public LayerMask GroundLayer {  get { return groundLayer; }}
    [SerializeField, Tooltip("플레이어 캐릭터 이동 속도")] private float moveSpeed;
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }
    [SerializeField, Tooltip("플레이어 캐릭터 점프력")] private float jumpForce;
    public float JumpForce
    {
        get { return jumpForce; }
        set { jumpForce = value; }
    }
    
    private BoxCollider2D boxCollider;

    // 플레이어 이동 방향
    private Vector2 moveDir = Vector2.zero;
    // 플레이어 캐릭터가 사다리에 닿은 상태일 때 true
    private bool touchLadder = false;
    public bool TouchLadder
    {
        get { return touchLadder; }
        set { touchLadder = value; }
    }
    // 플레이어 캐릭터가 땅에 닿으면 true
    private bool isGround;
    public bool IsGround { get  { return isGround; } }
    // 플레이어 캐릭터 조작 가능하면 true
    private bool isControllable = true;
    public bool IsControllable
    {
        get { return isControllable; }
        set { isControllable = value; }
    }

    private IFormState curState;
    public IFormState CurState
    {
        get { return curState; }
        set { curState = value; }
    }

    [Header("Push")]
    [SerializeField, Tooltip("플레이어 앞 박스를 감지할 거리")]
    private float pushDetectDistance = 0.05f;
    [SerializeField, Tooltip("밀 수 있는 박스 레이어")]
    private LayerMask pushableLayer;

    private IWeightable objWeight = null; 
    public IWeightable ObjWeight { get { return objWeight; } }
    // Ray로 감지한 물체의 무게
    private Rigidbody2D objRigid = null;
    public Rigidbody2D ObjRigid { get { return objRigid; } }
    // Ray로 감지한 물체의 rb

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        GroundCheck();
        if (curState != null)
        {
            curState.OnMove(moveDir, moveSpeed);
        }
    }

    void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down,
            0.02f, groundLayer);
        isGround = hit.collider != null ? true : false;
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (!isControllable) return;

        if (context.phase == InputActionPhase.Performed)
        {
            moveDir = context.ReadValue<Vector2>();
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            moveDir = Vector2.zero;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            Jump();
        }
    }

    public void Jump()
    {
        // 상태 패턴 구현 후 수정
        curState.OnJump();
    }

    private void WallRatationSet(Vector2 dir)
    {
        Vector3 rot;

        if(dir.x > 0)
        {
            rot = new Vector3(0, 0, 90);
        }
        else if(dir.x < 0)
        {
            rot = new Vector3(0, 0, -90);
        }
        else
        {
            rot = new Vector3(0, 0, transform.rotation.z);
        }

        transform.rotation = Quaternion.Euler(rot);
    }
    
    public bool TryDetectBox(Vector2 dir)
    {
        float xOffset = boxCollider.bounds.extents.x + 0.01f; 
        Vector2 origin = (Vector2)transform.position + new Vector2(Mathf.Sign(dir.x) * xOffset, 0);
        Vector2 direction = Vector2.right * Mathf.Sign(dir.x);

        // Debug.DrawRay(origin, direction * pushDetectDistance, Color.red, 1f);

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, pushDetectDistance, pushableLayer);
        if(hit.collider != null && hit.collider.TryGetComponent<IWeightable>(out var weight))
        // IWeightable이 붙은 컴포넌트인지 확인하고, 맞으면 True반환과 무게를 반환        
        {
            objWeight = weight;
            objRigid = hit.collider.attachedRigidbody;
            // Collider가 붙어있는 Rigidbody2D를 가져오고
            return true;
        }
        objWeight = null;
        objRigid = null;
        return false;
    }
}
