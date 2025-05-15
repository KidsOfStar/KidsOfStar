using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WirePuzzleSystem : MonoBehaviour
{
    [SerializeField, Tooltip("퍼즐 조각 프리팹")] private GameObject piecePrefab;
    [SerializeField, Tooltip("볼트 프리팹")] private GameObject boltPrefab;
    [SerializeField, Tooltip("퍼즐 조각이 배치될 패널")] private Transform puzzlePanel;
    [SerializeField, Tooltip("선택 영역의 RectTransform")] private RectTransform selectionBox;

    [SerializeField, Tooltip("퍼즐 가로 칸 수")] private int gridWidth = 4;
    [SerializeField, Tooltip("퍼즐 세로 칸 수")] private int gridHeight = 4;
    [SerializeField, Tooltip("조각의 크기")] private float cellSize = 75f;
    [SerializeField, Tooltip("padding 보정용")] private Vector2 offset = new Vector2(15f, 15f);

    #region 테스트용 임시 변수
    [Space, Header("테스트용 임시 변수들")]
    [SerializeField, Tooltip("테스트 퍼즐 조각용 스프라이트 배열")]
    private Sprite[] testSprites;
    [SerializeField, Tooltip("테스트용 퍼즐 데이터")] private WirePuzzleData puzzleData;
    #endregion

    // 퍼즐 조각 배열
    private WirePuzzlePiece[,] puzzleGrid;
    // 선택 영역의 좌표
    private int selectX = 0;
    private int selectY = 0;

    public void Init()
    {
        // 퍼즐 조각 배열 초기화
        puzzleGrid = new WirePuzzlePiece[gridWidth, gridHeight];
        GeneratePuzzle();
        SpawnBolt();
        UpdateSelectionBoxPosition();
        selectionBox.SetAsLastSibling();
        ShufflePuzzle();
        selectX = 0;
        selectY = 0;
    }

    private void Start()
    {
        Init();
    }

    void Update()
    {
        
    }

    public void SetupPuzzle()
    {

    }

    public void GeneratePuzzle()
    {
        // 퍼즐 조각 생성&배열에 배치
        for(int i = 0; i < gridHeight; i++)
        {
            for(int j = 0; j < gridWidth; j++)
            {
                // 퍼즐 조각 생성
                GameObject go = Instantiate(piecePrefab, puzzlePanel);
                WirePuzzlePiece piece = go.GetComponent<WirePuzzlePiece>();

                // 테스트용 스프라이트 적용
                Sprite spriet = testSprites[i * gridWidth + j];
                piece.InitPiece(j, i, spriet);
                piece.WireColor = GetColorType(j);
                puzzleGrid[j, i] = piece;
            }
        }
    }

    private WireColorType GetColorType(int x)
    {
        return x switch
        {
            0 => WireColorType.Yellow,
            1 => WireColorType.Blue,
            2 => WireColorType.Red,
            3 => WireColorType.Green,
            _ => WireColorType.Green
        };
    }

    public void StartPuzzle()
    {

    }

    public void SpawnBolt()
    {
        for (int i = 0; i < gridHeight - 1; i++)
        {
            for (int j = 0; j < gridWidth - 1; j++)
            {
                GameObject bolt = Instantiate(boltPrefab, puzzlePanel);
                RectTransform rt = bolt.GetComponent<RectTransform>();

                float x = j * cellSize + offset.x - ((gridWidth - 1) * cellSize / 2f);
                float y = -(i * cellSize + offset.y - ((gridHeight - 1) * cellSize / 2f));

                rt.anchoredPosition = new Vector2(x, y);

                int capturedX = j;
                int capturedY = i;

                Button btn = bolt.GetComponent<Button>();
                btn.onClick.AddListener(() => MoveSelection(capturedX, capturedY));
            }
        }
    }

    #region 작동을 위한 임시 코드
    // 생성된 퍼즐 조각 섞기
    private void ShufflePuzzle()
    {
        for(int i = 0; i < puzzleData.ShuffleCount; i++)
        {
            selectX = UnityEngine.Random.Range(0, gridWidth - 1);
            selectY = UnityEngine.Random.Range(0,gridHeight - 1);
            int rotateCount = UnityEngine.Random.Range(1, 3);

            for(int j = 0; j < rotateCount; j++)
            {
                RotateSelection();
            }
        }
    }

    // 선택 영역의 조각 회전
    private void MoveSelection(int dx, int dy)
    {
        //selectX = Mathf.Clamp(selectX + dx, 0, gridWidth - 2);
        //selectY = Mathf.Clamp(selectY + dy, 0, gridHeight - 2);
        selectX = Mathf.Clamp(dx, 0, gridWidth - 2);
        selectY =Mathf.Clamp(dy, 0, gridHeight - 2);
        UpdateSelectionBoxPosition();
    }

    // 선택 영역 좌표 수정
    private void UpdateSelectionBoxPosition()
    {
        float x = selectX * cellSize - ((gridWidth - 1) * cellSize / 2f);
        float y = -(selectY * cellSize - ((gridHeight - 1) * cellSize / 2f));
        selectionBox.anchoredPosition = new Vector2(x, y);
    }

    // 선택 영역 이동 버튼 연결 함수들
    public void OnMoveLeft() { MoveSelection(-1, 0); }
    public void OnMoveRight() { MoveSelection(1, 0); }
    public void OnMoveUp() { MoveSelection(0, -1); }
    public void OnMoveDown() { MoveSelection(0, 1); }

    // 선택 영역 내의 조각 스프라이트 시계방향으로 교체
    public void RotateSelection()
    {
        int x = selectX;
        int y = selectY;

        // 선택 영역 내의 조각 참조
        WirePuzzlePiece p1 = puzzleGrid[x, y];
        WirePuzzlePiece p2 = puzzleGrid[x + 1, y];
        WirePuzzlePiece p3 = puzzleGrid[x + 1, y + 1];
        WirePuzzlePiece p4 = puzzleGrid[x, y + 1];

        // 스프라이트 참조
        Sprite s1 = p1.GetSprite();
        Sprite s2 = p2.GetSprite();
        Sprite s3 = p3.GetSprite();
        Sprite s4 = p4.GetSprite();

        // 배선 색상 참조
        WireColorType c1 = p1.WireColor;
        WireColorType c2 = p2.WireColor;
        WireColorType c3 = p3.WireColor;
        WireColorType c4 = p4.WireColor;

        // 회전 적용
        p1.SetSprite(s4);
        p2.SetSprite(s1);
        p3.SetSprite(s2);
        p4.SetSprite(s3);

        p1.WireColor = c4;
        p2.WireColor = c1;
        p3.WireColor = c2;
        p4.WireColor = c3;

        if(CheckPuzzleClear())
        {
            EditorLog.Log("Clear");
        }
    }

    // 퍼즐 클리어 체크
    private bool CheckPuzzleClear()
    {
        for(int x = 0; x < gridWidth; x++)
        {
            WireColorType wColor = GetColorType(x);
            for(int y = 0; y < gridHeight; y++)
            {
                if (puzzleGrid[x, y].WireColor != wColor)
                    return false;
            }
        }

        return true;
    }
    #endregion
}