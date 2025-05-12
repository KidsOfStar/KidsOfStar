using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WirePuzzleSystem : MonoBehaviour
{
    [SerializeField, Tooltip("퍼즐 조각 프리팹")] private GameObject piecePrefab;
    [SerializeField, Tooltip("퍼즐 조각이 배치될 패널")] private Transform puzzlePanel;
    [SerializeField, Tooltip("선택 영역의 RectTransform")] private RectTransform selectionBox;

    [SerializeField, Tooltip("퍼즐 가로 칸 수")] private int gridWidth = 4;
    [SerializeField, Tooltip("퍼즐 세로 칸 수")] private int gridHeight = 4;
    [SerializeField, Tooltip("조각의 크기")] private float cellSize = 75f;

    // 퍼즐 조각 배열
    private WirePuzzlePiece[,] puzzleGrid;
    // 선택 영역의 좌표
    private int selectX = 0;
    private int selectY = 0;

    public void Init()
    {
        // 퍼즐 조각 배열 초기화
        puzzleGrid = new WirePuzzlePiece[gridWidth, gridHeight];

        // 퍼즐 조각 생성&배열에 배치
        for(int i = 0; i < gridHeight; i++)
        {
            for(int j = 0; j < gridWidth; j++)
            {
                // 퍼즐 조각 생성
                GameObject go = Instantiate(piecePrefab, puzzlePanel);
                WirePuzzlePiece piece = go.GetComponent<WirePuzzlePiece>();

            }
        }
    }

    void Update()
    {
        
    }
}
