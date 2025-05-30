using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SafePuzzleSystem : PuzzleSystemBase
{
    [Header("Background & Hint")]
    [SerializeField] private Image backgroundImage;    // SO.backgroundSprite 할당용
    [SerializeField] private GameObject easyModeOutline; // Easy 모드일 때만 켤 테두리

    [Header("Prefab & Layout")]
    [SerializeField] private GameObject piecePrefab;        
    [SerializeField] private Transform puzzleParent;

    public SafePuzzle safePuzzle;
    public SafePopup safePopup;

    public int safeIndex;                   // 금고 다이얼의 인덱스
    private List<Sprite> correctSprites;    // 정답 Sprite 목록
    private int gridWidth;                  // 퍼즐 배열 가로의 개수

    private int selectedIndex;
    public override int SelectedIndex => selectedIndex;  // 현재 선택된 퍼즐 조각의 Index
    private List<PuzzlePiece> pieces = new();   // 생성된 모든 퍼즐 조각목록
    public int ChallengeCount => challengeCount; // 현재 퍼즐 ID
    public bool IsPuzzleCleared { get; private set; } = false;  // 퍼즐이 클리어되었는지 여부

    public override void SetupPuzzle(ScriptableObject puzzleData, int puzzleId)
    {
        var data = puzzleData as TreePuzzleData;

        if (data == null)
        {
            EditorLog.LogWarning("TreePuzzleData가 아님");
            return;
        }
        this.puzzleIndex = puzzleId;
        correctSprites = new List<Sprite>(data.pieceSprites);
        gridWidth = data.gridWidth;

        bool isEasy = Managers.Instance.GameManager.Difficulty == Difficulty.Easy;
        timeLimit = isEasy ? data.easyTimeLimit : data.hardTimeLimit;

        if (backgroundImage != null)
            backgroundImage.sprite = data.backgroundSprite;

        if (easyModeOutline != null)
            easyModeOutline.SetActive(isEasy);
    }

    // 퍼즐 시작
    public override void StartPuzzle()
    {
        var sequence = puzzleIndex == 0 ? 13    
                     : puzzleIndex == 1 ? 15
                     : 0;
        if (sequence != 0)
            Managers.Instance.AnalyticsManager.SendFunnel(sequence.ToString());

        base.StartPuzzle();

        Managers.Instance.SoundManager.PlayBgm(BgmSoundType.InForestPuzzle);
        foreach (var piece in pieces)
        {
            piece.RandomizeRotation();
        }
    }
    // 조각 체크

    public override void CheckPuzzle()
    {
        foreach (var piece in pieces)
        {
            if (!piece.IsCorrect())
                return;
        }

        CompletePuzzle();
    }

    //퍼블 성공 (다 맞추면)
    protected override void CompletePuzzle()
    {
        IsPuzzleCleared = true; // 퍼즐 완료 상태 
        Managers.Instance.SoundManager.PlaySfx(SfxSoundType.PuzzleClear);
        SafeSetActive(safeIndex);

        if (!clearPuzzleSet.Contains(puzzleIndex))
        {
            clearPuzzleSet.Add(puzzleIndex);
        }

        safePopup.NextPuzzle();
    }

    //퍼즐 실패
    protected override void FailPuzzle()
    {
        Managers.Instance.UIManager.Hide<SafePopup>();
        base.FailPuzzle();
        ResetSystem();
    }

    public override void OnClearButtonClicked()
    {
        base.OnClearButtonClicked();
    }

    public override void OnExit()
    {
        base.OnExit();
        Managers.Instance.SoundManager.PlayBgm(BgmSoundType.Aquarium);
        Managers.Instance.UIManager.Hide<SafePopup>();
    }

    // 금고 다이얼 활성화
    private void SafeSetActive(int indexs)
    {
        safePuzzle.safeImage[indexs].raycastTarget = true;
        safePuzzle.safeImage[indexs].color = Color.white;
        safeIndex++;
    }

    //시스템 초기화
    public void ResetSystem()
    {
        safeIndex = 0;
        clearPuzzleSet.Clear();
        isRunning = false;

        foreach (var img in safePuzzle.safeImage)
        {
            img.raycastTarget = false;
            img.color = Color.gray;
        }

        safePuzzle.ResetPuzzleState(); // 퍼즐 상태 초기화
    }


    // 퍼즐 조각 생성
    public override void GeneratePuzzle()
    {
        IsPuzzleCleared = false; // 퍼즐 초기화

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
            PuzzlePiece piece = pieceGO.GetComponent<PuzzlePiece>();
            piece.SetSprite(correctSprites[i]);
            piece.Initialize(this, 0, i); // 정답각도는 0
            pieces.Add(piece);
        }
        HighlightSelectedPiece();
    }

    private void HighlightSelectedPiece()
    {
        for (int i = 0; i < pieces.Count; i++)
            pieces[i].SetHighlight(i == selectedIndex);
    }

    public override void OnPieceSelected(int index)
    {
        selectedIndex = index;
        HighlightSelectedPiece();
    }
}
