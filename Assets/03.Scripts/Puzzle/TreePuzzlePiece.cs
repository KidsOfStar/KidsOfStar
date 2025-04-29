using UnityEngine;
using UnityEngine.UI;

public class TreePuzzlePiece : MonoBehaviour
{
    private TreePuzzleSystem manager;
    
    [SerializeField] private Image pieceImage; // UI용 조각 이미지

    [SerializeField] private int correctRotation; // 0, 90, 180, 270

    private int currentRotation;
    private Outline outLine;

    private void Awake()
    {
      outLine = pieceImage.GetComponent<Outline>();                                    
    }
    public void Initialize(TreePuzzleSystem systemManager, int correctionRotation)
    {
        this.manager = systemManager;
        this.correctRotation = correctionRotation;
        currentRotation = 0;
    }

    public void RotateRight()
    {
        currentRotation = (currentRotation + 90) % 360;
        pieceImage.rectTransform.rotation = Quaternion.Euler(0, 0, -currentRotation);

        manager.CheckPuzzle();
    }

    public void RandomizeRotation()
    {
        currentRotation = 90 * Random.Range(1, 3);
        pieceImage.rectTransform.rotation = Quaternion.Euler(0, 0, -currentRotation);
    }

    public bool IsCorrect()
    {
        return currentRotation == correctRotation;
    }

    public void SetSprite(Sprite sprite)
    {
        pieceImage.sprite = sprite;
    }

    public void SetHighlight(bool on)
    {
        if (outLine != null)
            outLine.enabled = on;
    }
}


