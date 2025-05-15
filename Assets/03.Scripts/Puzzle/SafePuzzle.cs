using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SafePuzzle : MonoBehaviour, IPointerClickHandler
{
    public GameObject safeS;
    public GameObject safeM;
    public GameObject safeL;

    public Dictionary<GameObject, float> rotationAmount;                        // 퍼즐 조각과 회전량을 매핑하는 딕셔너리
    private HashSet<GameObject> completedPieces = new HashSet<GameObject>();    // 완료된 퍼즐 조각들

    void Start()
    {
        // 시작 시 랜덤으로 회전하기
        rotationAmount = new Dictionary<GameObject, float>
        {
            { safeS, 45f },
            { safeM, 60f },
            { safeL, 135f }
        };

         RandomizeRotation();
    }

    // 랜덤
    private void RandomizeRotation()
    {
        foreach (var pair in rotationAmount)
        {
            int randomMultiplier = UnityEngine.Random.Range(0, 3); // 0, 1, 2
            float randomRotation = pair.Value * randomMultiplier;
            pair.Key.transform.localEulerAngles = new Vector3(0, 0, randomRotation);    // z축을 기준으로 회전
        }
    }

    // 터치하여 각 퍼즐마다 오른쪽 혹은 왼쪽으로 회전하기
    // Rotation의 값이 0도 일 때 클리어
    // 각 퍼즐별로 회전하는 값이 다르며 0도 일 때 클리어

    // 퍼즐 조각을 클릭했을 때 회전하는 메서드
    public void RotatePuzzlePiece(GameObject puzzlePiece)
    {
        if (completedPieces.Contains(puzzlePiece)) return;      // 이미 완료된 퍼즐 조각은 회전하지 않음
        if (!rotationAmount.ContainsKey(puzzlePiece)) return;   // 퍼즐 조각이 딕셔너리에 없으면 회전하지 않음


        float amount = rotationAmount[puzzlePiece];
        puzzlePiece.transform.Rotate(0f, 0f, amount); // z축을 기준으로 회전

        CheckClearCondition();
    }

    private void CheckClearCondition()
    {
        foreach (var pair in rotationAmount)
        {
            float currentRotation = pair.Key.transform.localEulerAngles.z;
            float angle = Mathf.DeltaAngle(currentRotation, 0f);

            if (Mathf.Abs(angle) <= 1f)
            {
                completedPieces.Add(pair.Key);
            }
            else return;
        }

        // 모든 퍼즐 조각이 0도일 때 클리어
        Debug.Log("Puzzle Clear!");
    }

    // 퍼즐 조각을 클릭했을 때 회전하는 메서드
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject clickde = eventData.pointerCurrentRaycast.gameObject;
        if (rotationAmount.ContainsKey(clickde))
        {
            RotatePuzzlePiece(clickde);
        }
    }
}
