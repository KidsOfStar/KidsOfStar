using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SafePuzzle : MonoBehaviour, IPointerClickHandler
{
    public Image[] safeImage;
    public float rotationDuration = 0.5f;
    public SafePopup safePopup;
    public SafePuzzleTrigger safePuzzleSystem;

    // 모드별 제한 시간
    private int currentTime;
    private int safeNumber;

    public Dictionary<GameObject, float> rotationAmount;
    private HashSet<GameObject> completedPieces = new HashSet<GameObject>();
    private Dictionary<GameObject, Coroutine> rotationCorotines = new Dictionary<GameObject, Coroutine>();

    void Start()
    {
        safePuzzleSystem = FindObjectOfType<SafePuzzleTrigger>();

        rotationAmount = new Dictionary<GameObject, float>
        {
            { safeImage[0].gameObject, 90f },
            { safeImage[1].gameObject, 60f },
            { safeImage[2].gameObject, 135f }
        };

        RandomizeRotation();
    }

    private void OnEnable()
    {
        StartCoroutine(ClearTime());
    }

    private IEnumerator ClearTime()
    {
        currentTime = 0;   // 초기화
        // 1초마다 currentTime 증가
        WaitForSeconds oneSeconds = new WaitForSeconds(1f);

        while (true)
        {
            yield return oneSeconds;
            currentTime += 1;
        }
    }

    // 금고 다이얼 랜덤 배치 
    private void RandomizeRotation()
    {
        foreach (var pair in rotationAmount)
        {
            int randomMultiplier = UnityEngine.Random.Range(0, 3);
            float randomRotation = pair.Value * randomMultiplier;

            while (randomRotation == 0f) // 0도 포함되면 안됨
            {
                randomMultiplier = UnityEngine.Random.Range(0, 3);
                randomRotation = pair.Value * randomMultiplier;
            }

            pair.Key.transform.localEulerAngles = new Vector3(0, 0, randomRotation);
        }
    }

    public void RotatePuzzlePiece(GameObject puzzlePiece)
    {
        if (completedPieces.Contains(puzzlePiece)) return;
        if (!rotationAmount.ContainsKey(puzzlePiece)) return;
        if (rotationCorotines.ContainsKey(puzzlePiece)) return;

        float amount = rotationAmount[puzzlePiece];

        Coroutine coroutine = StartCoroutine(RotateOverTime(puzzlePiece, amount));
        rotationCorotines[puzzlePiece] = coroutine;

        CheckClearCondition();
    }

    // 퍼즐 조각을 부드럽게 회전시키는 코루틴
    private IEnumerator RotateOverTime(GameObject puzzlePiece, float amount)
    {
        // 현재 회전 상태 저장
        Quaternion startRotation = puzzlePiece.transform.rotation;

        // 목표 회전값 계산 (Z축 기준으로 amount만큼 회전)
        Quaternion endRotation = startRotation * Quaternion.Euler(0f, 0f, amount);

        // 경과 시간 초기화
        float elapsedTime = 0f;

        // 지정된 시간(duration) 동안 회전 수행
        while (elapsedTime < rotationDuration)
        {
            // 경과 시간 누적
            elapsedTime += Time.deltaTime;

            // 보간 비율 계산 (0에서 1 사이)
            float t = Mathf.Clamp01(elapsedTime / rotationDuration);

            // 현재 회전을 시작과 끝 사이에서 보간하여 적용 (부드러운 회전 효과)
            puzzlePiece.transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);

            // 다음 프레임까지 대기
            yield return null;
        }

        // 정확한 회전값 보정 (보간으로 인해 오차 발생 가능)
        puzzlePiece.transform.rotation = endRotation;

        // 회전 중인 코루틴 목록에서 제거
        rotationCorotines.Remove(puzzlePiece);

        // 퍼즐 클리어 조건 확인
        CheckClearCondition();
    }

    private void CheckClearCondition()
    {
        Quaternion target = Quaternion.Euler(0f, 0f, 0f);

        foreach (var pair in rotationAmount)
        {
            GameObject piece = pair.Key;
            if (completedPieces.Contains(piece)) continue;

            if (Quaternion.Angle(piece.transform.rotation, target) <= 1f)
            {
                completedPieces.Add(piece);
            }
        }

        // 퍼즐 완료 체크
        if (completedPieces.Count == rotationAmount.Count)
        {
            ClearPuzzle();
        }
    }
    // 퍼즐 완료 시 호출되는 메소드
    private void ClearPuzzle()
    {
        StopCoroutine(ClearTime());
        EditorLog.Log($"{currentTime}초 소요 - 퍼즐 완료");

        Managers.Instance.UIManager.Hide<SafePopup>();
        Managers.Instance.UIManager.Show<ClearPuzzlePopup>();

        UnlockDoor();

        Managers.Instance.GameManager.UpdateProgress();

        safePuzzleSystem.DisableExclamation();


        Managers.Instance.SoundManager.PlayBgm(BgmSoundType.Aquarium);
        Managers.Instance.SoundManager.PlayAmbience(AmbienceSoundType.Aquarium);

        var analyticsManager = Managers.Instance.AnalyticsManager;
        analyticsManager.RecordChapterEvent("PopUpPuzzle",
            ("PuzzleNumber", safeNumber),
            ("ChallengeCount", safePopup.challengeCount),
            ("ClearTime", currentTime)
            );

        if (safeNumber == 1)
        {
            analyticsManager.SendFunnel("42");
        }
        else if (safeNumber == 2)
        {
            analyticsManager.SendFunnel("43");
        }
        else if (safeNumber == 3)
        {
            analyticsManager.SendFunnel("45");
        }
    }

    private void UnlockDoor()
    {
        if (safePuzzleSystem.door != null)
        {
            safePuzzleSystem.door.isDoorOpen = true;
        }
    }


    public void SetSafeNumber(int number)
    {
        safeNumber = number;
    }

    // 클릭 이벤트 처리
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject clicked = eventData.pointerCurrentRaycast.gameObject;
        if (rotationAmount.ContainsKey(clicked))
        {
            RotatePuzzlePiece(clicked);
        }
    }

    // 퍼즐 상태 초기화
    public void ResetPuzzleState()
    {
        completedPieces.Clear();
        rotationCorotines.Clear();

        foreach (var pair in rotationAmount)
        {
            int randomMultiplier = UnityEngine.Random.Range(0, 3);
            float randomRotation = pair.Value * randomMultiplier;
            pair.Key.transform.localEulerAngles = new Vector3(0, 0, randomRotation);
        }
    }
}
