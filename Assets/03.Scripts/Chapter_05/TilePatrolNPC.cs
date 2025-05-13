using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePatrolNPC : MonoBehaviour
{
    public Tilemap tilemap; // 이동 경로를 구성하는 타일맵 참조
    public float moveSpeed = 3f; // NPC 이동 속도

    public List<Vector3> patrolPath = new(); // 타일 좌표(Vector3Int)로 구성된 순찰 경로
    private int currentIndex = 0;   // 현재 목표 타일 인덱스
    private bool isMoving = false; // NPC가 현재 이동 중인지 여부

    void Start()
    {
        // 경로가 하나라도 존재하면 시작 위치를 첫 타일로 정렬
        if (patrolPath.Count > 0)
        {
            transform.position = patrolPath[0]; // 타일 좌표를 월드 좌표로 변환
        }
    }
    void Update()
    {
        // 이동 중이 아니고, 순찰 경로가 존재할 때 다음 타일로 이동 시작
        if (!isMoving && patrolPath.Count > 0)
        {
            StartCoroutine(MoveToPosition(patrolPath[currentIndex]));
        }
    }

    // 특정 타일 좌표로 NPC를 이동시키는 코루틴
    IEnumerator MoveToPosition(Vector3 targetPos)
    {
        isMoving = true; // 이동 시작

        // 목표 위치에 도달할 때까지 프레임마다 MoveTowards로 이동
        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos; // 정확한 위치 보정
        currentIndex = (currentIndex + 1) % patrolPath.Count; // 다음 타일 인덱스로 순환
        yield return new WaitForSeconds(0.1f); // 약간의 대기 후 다음 이동
        isMoving = false; // 이동 종료
    }
}
