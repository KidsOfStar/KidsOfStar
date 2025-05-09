using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    None,
    Up,
    Down,
    Left,
    Right
}

// 물체를 감지할 때마다 MaxWeight를 초과하면 고장나는 코루틴이 시작되어야함
// 고장나면 움직이지 않음
// 일정시간 이후 다시 사용할 수 있어야함
// 잠긴 상태인지 아닌지 정해야함(퍼즐을 풀어야 함)

public class Elevator : MonoBehaviour
{
    // 이동속도 및 방향을 설정할 수 있어야 함
    [Header("Components")]
    [SerializeField] private Collider2D coll;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Rigidbody2D rigid;

    [Header("Elevator Settings")]
    [SerializeField] private bool isLocked = true;

    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float distance = 5.0f;
    [SerializeField] private Direction direction = Direction.None;

    private readonly WaitForFixedUpdate waitForFixedUpdate = new();
    private readonly WaitForSeconds moveWaitTime = new(0.5f);
    private readonly WaitForSeconds repairTime = new(5f);
    
    private readonly List<IWeightable> weightables = new();
    private const float MaxWeight = 3f;
    private const float VerticalMargin = 0.02f;

    private Vector3 startPos;
    private Vector3 targetPos;
    private bool isBroken = false;

    private void Start()
    {
        startPos = transform.position;
        targetPos = GetTargetPosition();

        if (!isLocked)
            StartCoroutine(Move());
    }

    private void UnlockElevator()
    {
        isLocked = false;
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (true)
        {
            yield return MoveRoutine(startPos, targetPos, true);
            yield return moveWaitTime;
            
            yield return MoveRoutine(targetPos, startPos);
            yield return moveWaitTime;
        }
    }

    private IEnumerator MoveRoutine(Vector3 from, Vector3 to, bool toTarget = false)
    {
        float distance = Vector3.Distance(from, to);
        float duration = distance / speed;
        float elapsed  = 0f;

        while (elapsed < duration)
        {
            // 과부하 감지: true면 BreakSequence 실행 후 리턴하지 않고 그대로 이 지점에서 재개
            if (GetCurrentWeight() > MaxWeight)
                yield return StartCoroutine(BreakSequence());

            // 진행도에 맞춰 위치 보간
            float t = elapsed / duration;
            Vector3 nextPos = Vector3.Lerp(from, to, t);
            
            rigid.MovePosition(nextPos);

            elapsed += Time.deltaTime;
            yield return waitForFixedUpdate;
        }

        // 정확히 to 위치 보정
        transform.position = to;

        if (toTarget && direction == Direction.Up)
        {
            
        }
    }

    private void FixedPlayerVelocity()
    {
        for (int i = 0; i < weightables.Count; i++)
        {
            var weightable = weightables[i];
            
        }
    }

    private IEnumerator BreakSequence()
    {
        const float blinkInterval = 0.5f; // 0.5초씩 색상 전환
        for (int i = 0; i < 3; i++)
        {
            // 1) White → Red
            float t = 0f;
            while (t < blinkInterval)
            {
                if (GetCurrentWeight() < MaxWeight)
                {
                    // 과부하 해제 시 즉시 종료
                    sprite.color = Color.white;
                    yield break;
                }

                sprite.color = Color.Lerp(Color.white, Color.red, t / blinkInterval);
                t += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
            sprite.color = Color.red;

            // 2) Red → White
            t = 0f;
            while (t < blinkInterval)
            {
                if (GetCurrentWeight() < MaxWeight)
                {
                    sprite.color = Color.white;
                    yield break;
                }

                sprite.color = Color.Lerp(Color.red, Color.white, t / blinkInterval);
                t += Time.deltaTime;
                yield return null;
            }

            sprite.color = Color.white;
        }
        
        // 3초 경고 후에도 과부하 상태라면 완전 고장
        isBroken = true;

        // 복구 대기
        yield return repairTime;

        // 고장 해제
        isBroken    = false;
        sprite.color = Color.white;
    }

    private Vector3 GetTargetPosition()
    {
        Vector3 targetPosition = startPos;

        switch (direction)
        {
            case Direction.Up:
                targetPosition.y += distance;
                break;
            case Direction.Down:
                targetPosition.y -= distance;
                break;
            case Direction.Left:
                targetPosition.x -= distance;
                break;
            case Direction.Right:
                targetPosition.x += distance;
                break;
            case Direction.None:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return targetPosition;
    }
    
    private float GetCurrentWeight()
    {
        float totalWeight = 0f;

        for (int i = 0; i < weightables.Count; i++)
        {
            var weightable = weightables[i];
            totalWeight += weightable.GetWeight();
        }
        
        return totalWeight;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (isBroken || isLocked) return;
        if (!other.gameObject.TryGetComponent(out IWeightable weightable)) return;

        // 물체가 엘레베이터에 올라탄 상태라면
        // 물체가 엘레베이터에 올라탄 상태가 아니라면
        if (IsOnElevator(other.collider))
        {
            if (weightables.Contains(weightable)) return;
            other.transform.SetParent(transform);
            weightables.Add(weightable);
        }
        else
        {
            other.transform.SetParent(null);
            if (weightables.Contains(weightable))
                weightables.Remove(weightable);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (!other.gameObject.TryGetComponent(out IWeightable weightable)) return;

        other.transform.SetParent(null);
        if (weightables.Contains(weightable))
            weightables.Remove(weightable);
    }

    private bool IsOnElevator(Collider2D weightable)
    {
        var obj = weightable.bounds;
        var elevator = coll.bounds;

        // 물체의 바닥면이 엘레베이터의 바닥면보다 아래에 있으면 false
        if (Mathf.Abs(obj.min.y - elevator.max.y) > VerticalMargin)
            return false;

        // 물체가 수평으로 반 이상 겹쳐있는지 확인
        float overlap = Mathf.Min(obj.max.x, elevator.max.x) - Mathf.Max(obj.min.x, elevator.min.x);
        overlap = Mathf.Max(overlap, 0f);

        return overlap >= obj.size.x * 0.5f;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}