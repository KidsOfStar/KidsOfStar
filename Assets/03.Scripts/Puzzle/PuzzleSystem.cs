using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PuzzleSystem : MonoBehaviour
{
    [Header("Prefab & Layout")]
    [SerializeField] private GameObject piecePrefab;
    [SerializeField] private Transform puzzleParent;

    [Header("Data")]
    [SerializeField] private List<Sprite> correctSprites;
    private List<PuzzlePiece> pieces = new List<PuzzlePiece>();

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerTxt;
    [SerializeField] private GameObject failPopup;

    [Header("Setting")]
    [SerializeField] private int gridWidth = 4;
    [SerializeField] private float timeLimit = 60f;

    private float currentTime;
    private bool isRunning = false;
    private int selectedIndex = 0;

    private void Start()
    {
        GeneratePuzzle();
        StartPuzzle();
    }

    void GeneratePuzzle()
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
            PuzzlePiece piece = pieceGO.GetComponent<PuzzlePiece>();
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
        // LoadScene을 통해서 다른 씬으로 진입이 가능하도록
    }

    private void FailPuzzle()
    {
        isRunning = false;
        failPopup.SetActive(true);
        gameObject.SetActive(false);
    }
}
