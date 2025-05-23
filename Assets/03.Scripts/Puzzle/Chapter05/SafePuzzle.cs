using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private IEnumerator RotateOverTime(GameObject puzzlePiece, float amount)
    {
        Quaternion startRotation = puzzlePiece.transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0f, 0f, amount);
        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / rotationDuration);
            puzzlePiece.transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            yield return null;
        }

        puzzlePiece.transform.rotation = endRotation;
        rotationCorotines.Remove(puzzlePiece);

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

    private void ClearPuzzle()
    {
        StopCoroutine(ClearTime());
        EditorLog.Log($"{currentTime}초 소요 - 퍼즐 완료");

        SafePuzzleClearData();

        Managers.Instance.UIManager.Hide<SafePopup>();
        Managers.Instance.UIManager.Show<ClearPuzzlePopup>();
        Managers.Instance.GameManager.UpdateProgress();

        safePuzzleSystem.DisableExclamation();

        AddObject();

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

    private void AddObject()
    {
        if (safePuzzleSystem.VentDoor != null)
        {
            safePuzzleSystem.VentDoor.SetActive(true);
        }

        else if (safePuzzleSystem.door != null)
        {
            safePuzzleSystem.door.isDoorOpen = true;
        }
    }


    public void SetSafeNumber(int number)
    {
        safeNumber = number;
        Debug.Log($"[SafePuzzle] 금고 번호 설정: {safeNumber}");
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

    // 퍼즐 클리어 데이터 저장
    private void SafePuzzleClearData()
    {
        int safeIndex = GetSafeIndexFromSceneType(safePuzzleSystem.sceneType);  // 0~2
        int puzzleIndex = safePopup.countIndex; // 0~2

        // 인덱스 범위 체크
        if (safeIndex >= 0 && safeIndex < 3 && puzzleIndex >= 0 && puzzleIndex < 3)
        {
            Managers.Instance.GameManager.clearedSafePuzzles[safeNumber, puzzleIndex] = true;
        }
        
    }
    // 씬 타입에 따라 안전한 금고 인덱스 반환
    private int GetSafeIndexFromSceneType(SceneType sceneType)
    {
        switch (sceneType)
        {
            case SceneType.Chapter501: return 0;
            case SceneType.Chapter502: return 1;
            case SceneType.Chapter504: return 2;
            default: return 0;
        }
    }
}
