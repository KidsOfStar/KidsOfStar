using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, ILeafJumpable
{
    [Header("공통")]
    [SerializeField] private LayerMask groundLayer;
    public LayerMask GroundLayer { get { return groundLayer; } }
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

    [Space, Header("고양이")]
    [SerializeField, Tooltip("벽 점프력")] private float wallJumpForce = 5f;
    public float WallJumpForce { get { return wallJumpForce; } }
    [SerializeField, Tooltip("벽에 붙었다가 떨어졌을 때 다시 붙을 수 있게 되기까지의 쿨타임")] private float catClingTimer = 1f;
    public float CatClingTimer { get { return catClingTimer; } }

    private Player player;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;

    // 플레이어 이동 방향
    private Vector2 moveDir = Vector2.zero;
    public Vector2 MoveDir { get { return moveDir; } }
    // 플레이어 캐릭터가 사다리에 닿은 상태일 때 true
    private bool touchLadder = false;
    public bool TouchLadder
    {
        get { return touchLadder; }
        set { touchLadder = value; }
    }
    // 플레이어 캐릭터가 땅에 닿으면 true
    private bool isGround;
    public bool IsGround { get { return isGround; } }
    // 플레이어 캐릭터 조작 가능하면 true
    private bool isControllable = true;
    public bool IsControllable
    {
        get { return isControllable; }
        set { isControllable = value; }
    }

    [Header("Push")]
    [SerializeField, Tooltip("플레이어 앞 박스를 감지할 거리")]
    private float pushDetectDistance = 0.05f;
    [SerializeField, Tooltip("밀 수 있는 박스 레이어")]
    private LayerMask pushableLayer;

    private IWeightable objWeight = null;
    // Ray로 감지한 물체의 무게
    private Rigidbody2D objRigid = null;
    // Ray로 감지한 물체의 rb

    private bool isLeafJumping = false;

    public void Init(Player player)
    {
        this.player = player;
        rigid = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
        Managers.Instance.CutSceneManager.OnCutSceneStart += LockPlayer;
        Managers.Instance.DialogueManager.OnDialogStart += LockPlayer;
        Managers.Instance.CutSceneManager.OnCutSceneEnd += UnlockPlayer;
        Managers.Instance.DialogueManager.OnDialogEnd += UnlockPlayer;
    }

    private void Update()
    {
        GroundCheck();
    }

    void GroundCheck()
    {
        Vector2 boxSize = new Vector2(boxCollider.bounds.size.x * 0.7f, boxCollider.bounds.size.y);
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down,
            0.02f, groundLayer);
        isGround = hit.collider != null ? true : false;
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (!isControllable) return;

        if (context.phase == InputActionPhase.Performed || context.phase == InputActionPhase.Started)
        {
            moveDir = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            moveDir = Vector2.zero;
        }
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Jump();
        }
    }

    public void OnFormChange(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        var skillBtn = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel;
        switch (context.control.name)
        {
            case "1":
                //player.FormControl.FormChange("Squirrel");
                skillBtn.OnSquirrel();
                break;
            case "2":
                //player.FormControl.FormChange("Dog");
                skillBtn.OnDog();
                break;
            case "3":
                //player.FormControl.FormChange("Cat");
                skillBtn.OnCat();
                break;
            case "4":
                //player.FormControl.FormChange("Hide");
                skillBtn.OnHide();
                break;
        }
    }

    public void Move()
    {
        if (moveDir != Vector2.up && moveDir != Vector2.down)
        {
            if (TryDetectBox(moveDir))
            {
                IPusher pusher = player.FormControl;
                float pushPower = pusher.GetPushPower();
                float objWeight = this.objWeight.GetWeight();
                float pushSpeed = (pushPower / objWeight) * moveSpeed;
                //미는 속도 = 미는 힘 / 무게 * 이동속도
                pushSpeed = Mathf.Min(pushSpeed, moveSpeed);
                // 미는 속도의 최대 이동속도 이상을 초과할 수 없도록

                Vector2 velocity = new Vector2(moveDir.x * pushSpeed, rigid.velocity.y);
                rigid.velocity = velocity;

                this.objRigid.velocity = velocity;
            }
            else
            {
                Vector2 playervelocity = new Vector2(moveDir.x * moveSpeed, rigid.velocity.y);
                rigid.velocity = playervelocity;
            }

            player.FormControl.FlipControl(moveDir);
        }
    }

    public void Jump()
    {
        if (!isControllable) return;

        if (isGround)
        {
            player.StateMachine.ChangeState(player.StateMachine.Factory.GetPlayerState(PlayerStateType.Jump));
        }
        else if(!player.StateMachine.ContextData.CanCling)
        {
            player.StateMachine.ChangeState(player.StateMachine.Factory.GetPlayerState(PlayerStateType.WallJump));
        }
    }

    public bool TryDetectBox(Vector2 dir)
    {
        float xOffset = boxCollider.bounds.extents.x + 0.01f;
        Vector2 origin = (Vector2)transform.position + new Vector2(Mathf.Sign(dir.x) * xOffset, 0.1f);
        Vector2 direction = Vector2.right * Mathf.Sign(dir.x);

        // Debug.DrawRay(origin, direction * pushDetectDistance, Color.red, 1f);

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, pushDetectDistance, pushableLayer);

        if (hit.collider != null)
        {
            if (hit.collider != null && hit.collider.TryGetComponent<IWeightable>(out var weight))
            // IWeightable이 붙은 컴포넌트인지 확인하고, 맞으면 True반환과 무게를 반환        
            {
                objWeight = weight;
                objRigid = hit.collider.attachedRigidbody;
                // Collider가 붙어있는 Rigidbody2D를 가져오고
                return true;
            }
        }
        objWeight = null;
        objRigid = null;
        return false;
    }

    //sprite 이미지에 맞춰서 콜라이더 생성
    public void SetCollider()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Bounds bounds = spriteRenderer.bounds;

        // bounds는 world size이므로, 로컬 좌표로 환산 필요
        Vector2 size = transform.InverseTransformVector(bounds.size);
        Vector2 offset = transform.InverseTransformPoint(bounds.center);

        boxCollider.size = size;
        boxCollider.offset = offset;
    }

    //플레이어 나뭇잎 점프
    public void StartLeafJump(Vector3 dropPosition, LayerMask groundMask, float moveSpeed, float jumpHeight)
    {
        if (isLeafJumping) return;
        StartCoroutine(LeafJumpRoutine(dropPosition, moveSpeed, jumpHeight));
    }

    private IEnumerator LeafJumpRoutine(Vector3 target, float moveSpeed, float jumpHeight)
    {
        isLeafJumping = true;
        var originalGravity = rigid.gravityScale;
        rigid.gravityScale = 0f; 
        rigid.velocity = Vector2.zero;

        Vector3 startPos = transform.position;
        float distance = Vector3.Distance(startPos, target);
        float duration = distance / moveSpeed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // x축·z축 선형 보간, y축은 사인 곡선
            Vector3 linearPos = Vector3.Lerp(startPos, target, t);
            float heightOffset = Mathf.Sin(t * Mathf.PI) * jumpHeight;
            transform.position = linearPos + Vector3.up * heightOffset;

            yield return null;
        }

        // 최종 위치 보정
        transform.position = target;
        rigid.gravityScale = originalGravity;
        isLeafJumping = false;
    }
    
    private void LockPlayer()
    {
        isControllable = false;
    }
    
    private void UnlockPlayer()
    {
        isControllable = true;
    }

    private void OnDestroy()
    {
        Managers.Instance.CutSceneManager.OnCutSceneStart -= LockPlayer;
        Managers.Instance.CutSceneManager.OnCutSceneEnd -= UnlockPlayer;
        
        Managers.Instance.DialogueManager.OnDialogStart -= LockPlayer;
        Managers.Instance.DialogueManager.OnDialogEnd -= UnlockPlayer;
    }
}
