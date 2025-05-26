using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreePuzzleSystem : PuzzleSystemBase
{
    [Header("Background & Hint")]
    [SerializeField] private Image backgroundImage;    // SO.backgroundSprite 할당용
    [SerializeField] private GameObject easyModeOutline; // Easy 모드일 때만 켤 테두리

    [Header("Prefab & Layout")]
    [SerializeField] private GameObject piecePrefab;
    [SerializeField] private Transform puzzleParent;

    public bool IsRunning => isRunning;
    // 정답 Sprite 목록
    private List<Sprite> correctSprites;
    // 퍼즐 배열 가로의 개수
    private int gridWidth;
    // 현재 선택된 퍼즐 조각의 Index
    private int selectedIndex;
    public int SelectedIndex => selectedIndex;  
    // 생성된 모든 퍼즐 조각목록
    private List<TreePuzzlePiece> pieces = new();
    // 성공 완료된 퍼즐 ID의 집합
    protected HashSet<int> clearPuzzlenum = new();

    // 퍼즐 준비
    public override void SetupPuzzle(ScriptableObject puzzleData, int index)
    {
        var data = puzzleData as TreePuzzleData;

        if (data == null)
        {
            EditorLog.LogWarning("TreePuzzleData가 아님");
            return;
        }
        puzzleIndex = index;
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
    public override void GeneratePuzzle()
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
            piece.Initialize(this, 0,i); // 정답각도는 0
            pieces.Add(piece);
        }
        HighlightSelectedPiece();
    }

    // 퍼즐 시작
    public override void StartPuzzle()
    {
        var sequence = puzzleIndex == 0 ? 13
                     : puzzleIndex == 1 ? 15
                     : 0;
        if (sequence != 0)
            Managers.Instance.AnalyticsManager.SendFunnel(sequence.ToString());

        challengeCount++;

        currentTime = timeLimit;
        isRunning = true;
        Managers.Instance.SoundManager.PlayBgm(BgmSoundType.InForestPuzzle);
        foreach (var piece in pieces)
        {
            piece.RandomizeRotation();
        }
    }

    // 퍼즐 조각 클릭시 실행되는 아웃라인 및 회전 메서드
    public void OnPieceSelected(int index)
    {
        selectedIndex = index;
        HighlightSelectedPiece();
    }

    // 선택한 퍼즐 아웃라인표시
    private void HighlightSelectedPiece()
    {
        for (int i = 0; i < pieces.Count; i++)
            pieces[i].SetHighlight(i == selectedIndex);
    }

    // 조각 체크
    public void CheckPuzzle()
    {
        foreach (var piece in pieces)
        {
            if (!piece.IsCorrect())
                return;
        }

        CompletePuzzle();
    }

    //퍼즐 Clear시
    protected virtual void CompletePuzzle()
    {
        isRunning = false;

        Managers.Instance.SoundManager.PlaySfx(SfxSoundType.PuzzleClear);

        Managers.Instance.UIManager.Hide<TreePuzzlePopup>();
        Managers.Instance.UIManager.Show<ClearPuzzlePopup>(this);
        // OnExit();

        EditorLog.Log("퍼즐 성공!");
        if (!clearPuzzlenum.Contains(puzzleIndex))
        {
            clearPuzzlenum.Add(puzzleIndex);
        }

        int clearTime = Mathf.CeilToInt(timeLimit - currentTime);
        var analyticsManager = Managers.Instance.AnalyticsManager;
        var fallNum = Managers.Instance.AnalyticsManager.fallCount;

        if (Managers.Instance.GameManager.CurrentChapter == ChapterType.Chapter2)
        {
            analyticsManager.RecordChapterEvent("MapPuzzle",
                                               ("PuzzleNumber", puzzleIndex),
                                               ("FallCount", fallNum));
        }

        analyticsManager.RecordChapterEvent("PopUpPuzzle",
                                           ("PuzzleNumber", puzzleIndex),
                                           ("ChallengeCount", challengeCount),
                                           ("ClearTime", clearTime));
        challengeCount = 0;
        Managers.Instance.AnalyticsManager.fallCount = 0;

        var sequence = puzzleIndex == 1 ? 14
                     : puzzleIndex == 2 ? 16
                     : -1;

        if (sequence > 0)
            analyticsManager.SendFunnel(sequence.ToString());
    }

    public virtual void OnClearButtonClicked()
    {
        var trigger = Managers.Instance.PuzzleManager.GetTrigger(puzzleIndex);
        trigger?.DisableExclamation();

        // 팝업 닫고 플레이어 제어 복구
        OnExit();

        // clearPuzzlenum.Count 에 따라 컷신 분기 재생
        if (clearPuzzlenum.Count == 1)
        {
            Managers.Instance.CutSceneManager.PlayCutScene(CutSceneType.DaunRoom);
            Managers.Instance.GameManager.UpdateProgress();
        }
        else if (clearPuzzlenum.Count >= totalPuzzleCount)
        {
            Managers.Instance.CutSceneManager.PlayCutScene(CutSceneType.LeavingForest);
            Managers.Instance.GameManager.UpdateProgress();
            Managers.Instance.AnalyticsManager.SendFunnel("17");
        }
    }

    // UI닫기
    public override void OnExit()
    {
        Managers.Instance.SoundManager.PlayBgm(BgmSoundType.InForest);
        base.OnExit();
    }
}