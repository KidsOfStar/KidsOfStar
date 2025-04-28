using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TreePuzzleSystem : MonoBehaviour
{
    [Header("Background & Hint")]
    [SerializeField] private Image backgroundImage;    // SO.backgroundSprite 할당용
    [SerializeField] private GameObject easyModeOutline; // Easy 모드일 때만 켤 테두리

    [Header("Prefab & Layout")]
    [SerializeField] private GameObject piecePrefab;
    [SerializeField] private Transform puzzleParent;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerTxt;
    [SerializeField] private GameObject failPopup;
    [SerializeField] private GameObject ClearPopup;
    [SerializeField] private Button clearExitBtn;

    // 정답 Sprite 목록
    private List<Sprite> correctSprites;
    // 퍼즐 배열 가로의 개수
    private int gridWidth;
    // 모드별 제한 시간
    private float timeLimit;

    private float currentTime;
    // 작동중인지 체크
    private bool isRunning;
    // 현재 선택된 퍼즐 조각의 Index
    private int selectedIndex;
    // 생성된 모든 퍼즐 조각목록
    private List<TreePuzzlePiece> pieces = new();
    // 퍼즐 고유ID
    private int puzzleIndex;
    // 성공 완료된 퍼즐 ID의 집합
    private HashSet<int> clearPuzzlenum = new();
    // 에디터에서 전체 퍼즐의 개수
    [SerializeField] private int totalPuzzleCount = 2;
    // Trigger형태를 저장한 딕셔너리
    private Dictionary<int, TreePuzzleTrigger> triggerMap;
    private void Awake()
    {
        triggerMap = new Dictionary<int, TreePuzzleTrigger>();
        foreach (var trig in FindObjectsOfType<TreePuzzleTrigger>())
        {
            triggerMap[trig.SequenceIndex] = trig;
        }
    }

    // 퍼즐 준비
    public void SetupPuzzle(TreePuzzleData data, int puzzleClearIndex)
    {
        if (ClearPopup != null)
        {
            ClearPopup.SetActive(false);
        }
        if (failPopup != null)
        {
            failPopup.SetActive(false);
        }

        puzzleIndex = puzzleClearIndex;
        correctSprites = new List<Sprite>(data.pieceSprites);
        gridWidth = data.gridWidth;
        bool isEasy = Managers.Instance.GameManager.Difficulty == Difficulty.Easy;
        timeLimit = isEasy ? data.easyTimeLimit : data.hardTimeLimit;

        if (backgroundImage != null)
            backgroundImage.sprite = data.backgroundSprite;

        if (easyModeOutline != null)
            easyModeOutline.SetActive(isEasy);

    }

    // 퍼즐 조각 생성
    public void GeneratePuzzle()
    {
        selectedIndex = 0;
        // 기존 조각 제거
        foreach (Transform child in puzzleParent)
        {
            Destroy(child.gameObject);
        }
        pieces.Clear();

        // 퍼즐 조각 생성
        for (int i = 0; i < correctSprites.Count; i++)
        {
            GameObject pieceGO = Instantiate(piecePrefab, puzzleParent);
            TreePuzzlePiece piece = pieceGO.GetComponent<TreePuzzlePiece>();
            piece.SetSprite(correctSprites[i]);
            piece.Initialize(this, 0); // 정답각도는 0
            pieces.Add(piece);
        }
        HighlightSelectedPiece();
    }

    private void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime;
        timerTxt.text = Mathf.CeilToInt(currentTime).ToString();

        if (currentTime <= 0f)
        {
            FailPuzzle();
        }
    }

    // 퍼즐 시작
    public void StartPuzzle()
    {
        currentTime = timeLimit;
        isRunning = true;

        foreach (var piece in pieces)
        {
            piece.RandomizeRotation();
        }
    }

    // 퍼즐선택방향키(1차원 리스트 활용)
    public void MoveSelection(string direction)
    {
        switch (direction)
        {
            case "Up":
                if (selectedIndex - gridWidth >= 0)
                    selectedIndex -= gridWidth;
                break;
            case "Down":
                if (selectedIndex + gridWidth < pieces.Count)
                    selectedIndex += gridWidth;
                break;
            case "Left":
                if (selectedIndex % gridWidth != 0)
                    selectedIndex -= 1;
                break;
            case "Right":
                if ((selectedIndex + 1) % gridWidth != 0)
                    selectedIndex += 1;
                break;
        }
        EditorLog.Log(direction);
        HighlightSelectedPiece();
    }

    // 선택한 퍼즐 90도 회전
    public void RotateSelectedPiece()
    {
        pieces[selectedIndex].RotateRight();
    }

    // 선택한 퍼즐 아웃라인표시
    private void HighlightSelectedPiece()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            pieces[i].SetHighlight(i == selectedIndex);
        }
    }

    // 조각 체크
    public void CheckPuzzle()
    {
        //foreach (var piece in pieces)
        //{
        //    if (!piece.IsCorrect())
        //        return;
        //}
        CompletePuzzle();
    }

    //퍼즐 Clear시
    private void CompletePuzzle()
    {
        isRunning = false;
        ClearPopup.SetActive(true);

        EditorLog.Log("퍼즐 성공!");
        if (!clearPuzzlenum.Contains(puzzleIndex))
        {
            clearPuzzlenum.Add(puzzleIndex);
        }
        clearExitBtn.onClick.RemoveAllListeners();
        clearExitBtn.onClick.AddListener(OnClearButtonClicked);
    }

    // 퍼즐 실패시
    private void FailPuzzle()
    {
        isRunning = false;
        failPopup.SetActive(true);

        if (triggerMap.TryGetValue(puzzleIndex, out var trig))
        {
            trig.ResetTrigger();
        }
    }
    
    //퍼즐 취소시
    public void StopPuzzle()
    {
        isRunning = false;

        if (failPopup != null)
            failPopup.SetActive(false);
        if (ClearPopup != null)
            ClearPopup.SetActive(false);

        if (triggerMap.TryGetValue(puzzleIndex, out var trig))
            trig.ResetTrigger();
    }

    public void OnClearButtonClicked()
    {
        if (triggerMap.TryGetValue(puzzleIndex, out var trig))
        {
            trig.DisableExclamation();
        }

        // 팝업 닫고 플레이어 제어 복구
        OnExit();

        // clearPuzzlenum.Count 에 따라 컷신 분기 재생
        if (clearPuzzlenum.Count == 1)
        {
            Managers.Instance.CutSceneManager.PlayCutScene(CutSceneType.DaunRoom.GetName());
            Managers.Instance.GameManager.UpdateProgress();
        }
        else if (clearPuzzlenum.Count >= totalPuzzleCount)
        {
            Managers.Instance.CutSceneManager.PlayCutScene(CutSceneType.LeavingForest.GetName());
            Managers.Instance.GameManager.UpdateProgress();
        }
    }

    // UI닫기
    public void OnExit()
    {
        Managers.Instance.UIManager.Hide<TreePuzzlePopup>();
        Managers.Instance.GameManager.Player.Controller.IsControllable = true;
    }
}