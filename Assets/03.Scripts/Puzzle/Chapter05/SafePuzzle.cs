using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SafePuzzle : MonoBehaviour, IPointerClickHandler
{
    public Image []safeImage;

    public float rotationDuration = 0.5f; // 회전하는 데 걸리는 시간

    public SafePopup safePopup; // 퍼즐 팝업을 참조하기 위한 변수

    public Dictionary<GameObject, float> rotationAmount;                        // 퍼즐 조각과 회전량을 매핑하는 딕셔너리
    private HashSet<GameObject> completedPieces = new HashSet<GameObject>();    // 완료된 퍼즐 조각들
    private Dictionary<GameObject, Coroutine> rotationCorotines = new Dictionary<GameObject, Coroutine>(); // 천천히 회전하는 코루틴을 저장하는 딕셔너리
    
    void Start()
    {
        // 시작 시 랜덤으로 회전하기
        rotationAmount = new Dictionary<GameObject, float>
        {
            { safeImage[0].gameObject, 45f },
            { safeImage[1].gameObject, 60f },
            { safeImage[2].gameObject, 135f }
        };

         RandomizeRotation();
    }

    // 회전 랜덤 값을 설정하는 메서드
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

        if (rotationCorotines.ContainsKey(puzzlePiece)) return; // 이미 회전 중인 퍼즐 조각은 회전하지 않음

        float amount = rotationAmount[puzzlePiece];

        Coroutine coroutine = StartCoroutine(RotateOverTime(puzzlePiece, amount));
        puzzlePiece.transform.Rotate(0f, 0f, amount); // z축을 기준으로 회전

        CheckClearCondition();
    }

    // 퍼즐 조각을 천천히 회전하는 코루틴
    private IEnumerator RotateOverTime(GameObject puzzlePiece, float amount)
    {
        Quaternion startRotation = puzzlePiece.transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0f, 0f, amount);

        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / rotationDuration);    // 0f ~ 1f 사이의 값으로 보간
            puzzlePiece.transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            yield return null;
        }

        puzzlePiece.transform.rotation = endRotation; // 최종 회전값으로 설정

        rotationCorotines.Remove(puzzlePiece);


        CheckClearCondition();
    }

    private void CheckClearCondition()
    {
        Quaternion target = Quaternion.Euler(0f, 0f, 0f);

        foreach (var pair in rotationAmount)
        {
            GameObject piece = pair.Key;

            if (completedPieces.Contains(piece)) continue; // 이미 완료된 퍼즐은 스킵

            Quaternion currentRotation = piece.transform.rotation;

            if (Quaternion.Angle(currentRotation, target) <= 1f)
            {
                completedPieces.Add(piece);
                Debug.Log($"{piece.name} 퍼즐 완료!");
            }
        }

        // 모든 퍼즐 완료됐는지 확인
        if (completedPieces.Count == rotationAmount.Count)
        {
            Debug.Log("Puzzle Clear!");
        }
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
