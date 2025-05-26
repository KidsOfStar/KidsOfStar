using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Puzzle/TreePuzzleData")]
public class TreePuzzleData : ScriptableObject, IPuzzleData
{
    public string puzzleId;
    [field : SerializeField] public Sprite BackGroundSprite { get; private set; }

    public Sprite backgroundSprite => BackGroundSprite;

    public List<Sprite> pieceSprites;
    public int gridWidth = 4;
    public float easyTimeLimit = 90f;
    public float hardTimeLimit = 90f;
}
