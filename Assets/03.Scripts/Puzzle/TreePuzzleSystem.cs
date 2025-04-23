using System.Collections.Generic;
using TMPro;
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

    private List<Sprite> correctSprites;
    private int gridWidth;
    private float timeLimit;

    private float currentTime;
    private bool isRunning;
    private int selectedIndex;
    private List<TreePuzzlePiece> pieces = new();

    private int puzzleIndex;
    private HashSet<int> clearPuzzlenum = new();
    [SerializeField] private int totalPuzzleCount = 2;

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

    public void GeneratePuzzle()
    {
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
    public void StartPuzzle()
    {
        currentTime = timeLimit;
        isRunning = true;

        foreach (var piece in pieces)
        {
            piece.RandomizeRotation();
        }
    }

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

    public void RotateSelectedPiece()
    {
        pieces[selectedIndex].RotateRight();
    }

    private void HighlightSelectedPiece()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            pieces[i].SetHighlight(i == selectedIndex);
        }
    }

    public void CheckPuzzle()
    {
        foreach (var piece in pieces)
        {
            if (!piece.IsCorrect())
                return;
        }
        CompletePuzzle();
    }

    private void CompletePuzzle()
    {
        isRunning = false;
        ClearPopup.SetActive(true);
        
        EditorLog.Log("퍼즐 성공!");
        if (!clearPuzzlenum.Contains(puzzleIndex))
        {
            clearPuzzlenum.Add(puzzleIndex);
        }

        if (clearPuzzlenum.Count >= totalPuzzleCount)
        {
            EditorLog.Log("모든 퍼즐 클리어! 추가 로직 실행");
            // Managers.Instance.CutSceneManager.PlayCutScene(...);
        }
        else
        {
            EditorLog.Log($"{puzzleIndex} 첫 클리어");
        }
        //플레이하고자 하는 컷씬의 이름으로 로드
    }

    private void FailPuzzle()
    {
        isRunning = false;
        failPopup.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnExit()
    {
        Managers.Instance.UIManager.Hide<TreePuzzlePopup>();
    }
}