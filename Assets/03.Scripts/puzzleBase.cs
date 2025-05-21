using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class puzzleBase : MonoBehaviour
{
    [Header("setting")]
    [SerializeField] protected Image backgroundImage;
    [SerializeField] protected GameObject outLine;

    [Header("Prefab")]
    [SerializeField] protected GameObject piecePrefab;
    [SerializeField] private Transform puzzleParent;

    [Header("UI")]
    [SerializeField] protected TextMeshProUGUI timerTxt;

    protected float timeLimit;
    protected float currentTime;
    protected int gridWidth = 4;
    protected bool isRunning;
    protected int challengeCount;

    // 정답 Sprite 목록
    private List<Sprite> correctSprites;

    // 생성된 모든 퍼즐 조각목록
    private List<TreePuzzlePiece> pieces = new();

    // 퍼즐 고유ID
    protected int puzzleIndex;

    // 현재 선택된 퍼즐 조각의 Index
    protected int selectedIndex;

    // Trigger형태를 저장한 딕셔너리
    protected Dictionary<int, PuzzleTrigger> triggerMap;


    // 퍼즐 준비
    public virtual void SetupPuzzle(TreePuzzleData data, int puzzleClearIndex) // PuzzleData로 수정
    {
        puzzleIndex = puzzleClearIndex;
        correctSprites = new List<Sprite>(data.pieceSprites);
        gridWidth = data.gridWidth;
        bool isEasy = Managers.Instance.GameManager.Difficulty == Difficulty.Easy;
        timeLimit = isEasy ? data.easyTimeLimit : data.hardTimeLimit;

        if (backgroundImage != null)
            backgroundImage.sprite = data.backgroundSprite;

        if (outLine != null)
            outLine.SetActive(isEasy);
    }

    // 퍼즐 조각 생성
    public virtual void GeneratePuzzle()
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
            piece.Initialize(this, 0, i); // 정답각도는 0
            pieces.Add(piece);
        }
        HighlightSelectedPiece();
    }

    private void Update()  //TODO: 코루틴으로 변경
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
    public virtual void StartPuzzle()
    {
        //var sequence = puzzleIndex == 0 ? 13
        //             : puzzleIndex == 1 ? 15
        //             : 0;
        //if (sequence != 0)
        //    Managers.Instance.AnalyticsManager.SendFunnel(sequence.ToString());

        challengeCount++;

        currentTime = timeLimit;
        isRunning = true;
        // Managers.Instance.SoundManager.PlayBgm(BgmSoundType.InForestPuzzle);
        foreach (var piece in pieces)
        {
            piece.RandomizeRotation();
        }
    }

    // 퍼즐 조각 클릭시 실행되는 아웃라인 및 회전 메서드
    // TODO: YDY만 쓸 예정
    //public void OnPieceSelected(int index)
    //{
    //    selectedIndex = index;
    //    HighlightSelectedPiece();
    //}

    // 선택한 퍼즐 아웃라인표시
    protected virtual void HighlightSelectedPiece()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            pieces[i].SetHighlight(i == selectedIndex);
        }
    }

    // 조각 체크
    public virtual void CheckPuzzle()
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
        puzzleIndex = 0;
        Managers.Instance.AnalyticsManager.fallCount = 0;

        var sequence = puzzleIndex == 1 ? 14
                     : puzzleIndex == 2 ? 16
                     : 0;

        if (sequence != 0)
            analyticsManager.SendFunnel(sequence.ToString());
    }

    // 퍼즐 실패시
    protected virtual void FailPuzzle()
    {
        isRunning = false;
        Managers.Instance.SoundManager.PlaySfx(SfxSoundType.PuzzleFail);

        Managers.Instance.UIManager.Hide<TreePuzzlePopup>();
        Managers.Instance.UIManager.Show<GameOverPopup>();
        OnExit();

        if (triggerMap.TryGetValue(puzzleIndex, out var trig))
        {
            trig.ResetTrigger();
        }
    }

    //퍼즐 취소시
    public void StopPuzzle()
    {
        isRunning = false;

        Managers.Instance.UIManager.Hide<TreePuzzlePopup>();

        if (triggerMap.TryGetValue(puzzleIndex, out var trig))
            trig.ResetTrigger();
    }

    public virtual void OnClearButtonClicked()
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
    public virtual void OnExit()
    {
        Managers.Instance.SoundManager.PlayBgm(BgmSoundType.InForest);
        Managers.Instance.GameManager.Player.Controller.UnlockPlayer();
    }
}
