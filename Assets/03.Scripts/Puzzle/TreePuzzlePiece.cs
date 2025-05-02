using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TreePuzzlePiece : MonoBehaviour, IPointerClickHandler
{
    private TreePuzzleSystem manager;
    private int curIndex;

    [SerializeField] private Image pieceImage; // UI용 조각 이미지

    [SerializeField] private int correctRotation; // 0, 90, 180, 270

    private int currentRotation;
    private Outline outLine;

    private void Awake()
    {
        outLine = pieceImage.GetComponent<Outline>();
        pieceImage.raycastTarget = true;
    }
    public void Initialize(TreePuzzleSystem systemManager, int correctionRotation, int index)
    {
        manager = systemManager;
        correctRotation = correctionRotation;
        currentRotation = 0;
        curIndex = index;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!manager.IsRunning) return;

        if (manager.SelectedIndex == curIndex)
        {
            RotateRight();
        }
    }

    public void RotateRight()
    {
        Managers.Instance.SoundManager.PlaySfx(SfxSoundType.TurnPuzzle);
        currentRotation = (currentRotation + 90) % 360;
        pieceImage.rectTransform.rotation = Quaternion.Euler(0, 0, -currentRotation);

        manager.CheckPuzzle();
    }

    public void RandomizeRotation()
    {
        currentRotation = 90 * Random.Range(1, 4);
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


