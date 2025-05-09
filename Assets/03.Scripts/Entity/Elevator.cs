using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Collections;
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
    [SerializeField] private Collider2D elevatorCollider;

    [Header("Elevator Settings")]
    [SerializeField] private bool isLocked = true;

    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float distance = 5.0f;
    [SerializeField] private Direction direction = Direction.None;

    private readonly WaitForSeconds moveWaitTime = new(0.5f);
    private const float MaxWeight = 3f;
    private const float VerticalMargin = 0.02f;

    private Vector3 startPos;
    private bool isBroken = false;

    private void Start()
    {
        startPos = transform.position;

        if (!isLocked)
            StartCoroutine(MoveCoroutine());
    }

    private void UnlockElevator()
    {
        isLocked = false;
        StartCoroutine(MoveCoroutine());
    }

    private IEnumerator MoveCoroutine()
    {
        Vector3 targetPos = GetTargetPosition();

        while (!isBroken)
        {
            yield return moveWaitTime; // 대기

            // 목표 위치까지 이동
            float elapsedTime = 0f;
            while (elapsedTime < distance / speed)
            {
                transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / (distance / speed));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPos;

            yield return moveWaitTime; // 대기

            // 원래 위치로 이동
            elapsedTime = 0f;
            while (elapsedTime < distance / speed)
            {
                transform.position = Vector3.Lerp(targetPos, startPos, elapsedTime / (distance / speed));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = startPos;
        }
    }

    private Vector3 GetTargetPosition()
    {
        Vector3 targetPos = startPos;

        switch (direction)
        {
            case Direction.Up:
                targetPos.y += distance;
                break;
            case Direction.Down:
                targetPos.y -= distance;
                break;
            case Direction.Left:
                targetPos.x -= distance;
                break;
            case Direction.Right:
                targetPos.x += distance;
                break;
            case Direction.None:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return targetPos;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (isBroken) return;
        if (!other.gameObject.TryGetComponent(out IWeightable weightable)) return;

        // 물체가 엘레베이터에 올라탄 상태라면
        // 물체가 엘레베이터에 올라탄 상태가 아니라면
        other.transform.SetParent(IsOnElevator(other.collider) ? transform : null);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (isBroken) return;
        if (!other.gameObject.TryGetComponent(out IWeightable weightable)) return;

        other.transform.SetParent(null);
    }

    private bool IsOnElevator(Collider2D weightable)
    {
        var obj = weightable.bounds;
        var elevator = elevatorCollider.bounds;

        // 물체의 바닥면이 엘레베이터의 바닥면보다 아래에 있으면 false
        EditorLog.Log("수평체크: " + Mathf.Abs(obj.min.y - elevator.max.y));
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