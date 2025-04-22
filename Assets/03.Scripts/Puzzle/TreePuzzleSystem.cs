using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TreePuzzleSystem : MonoBehaviour
{
    [Header("Prefab & Layout")]
    [SerializeField] private GameObject piecePrefab;
    [SerializeField] private Transform puzzleParent;

    [Header("Data")]
    [SerializeField] private List<Sprite> correctSprites;
    private List<TreePuzzlePiece> pieces = new ();

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerTxt;
    [SerializeField] private GameObject failPopup;
    [SerializeField] private GameObject easyModeOutline;

    [Header("Setting")]
    [SerializeField] private int gridWidth = 4;
    public bool isEasyMode;

    private TreePuzzleTrigger puzzleTrigger;
    private float timeLimit = 60f;
    private float currentTime;
    private bool isRunning = false;
    private int selectedIndex = 0;

    public void InitPuzzle()
    {
        isEasyMode = Managers.Instance.GameManager.Difficulty == Difficulty.Easy; //여기서 난이도 설정
        timeLimit = isEasyMode ? 180f : 90f;

        if (easyModeOutline != null)
            easyModeOutline.SetActive(isEasyMode);
    }

    public void GeneratePuzzle()
    {
        InitPuzzle();

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
        EditorLog.Log("퍼즐 성공!");
        gameObject.SetActive(false);

        string id = puzzleTrigger.PuzzleId;
        if (!Managers.Instance.GameManager.IsPuzzleCleared(id))
        {
            Managers.Instance.GameManager.AddPuzzleCleared(id);
        }

        if (Managers.Instance.GameManager.GetPuzzleClearCount() >= 2)
        {
            //플레이하고자 하는 컷씬의 이름으로 로드
            //Managers.Instance.CutSceneManager.PlayCutScene(CutSceneType..GetName());  
        }
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