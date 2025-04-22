using UnityEngine;
using UnityEngine.UI;

public class TreePuzzlePopup : PopupBase
{
    [SerializeField] private Button upButton;
    [SerializeField] private Button downButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button rotateButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private TreePuzzleSystem currentPuzzle;

    private void Awake()
    {
        upButton.onClick.RemoveAllListeners();
        downButton.onClick.RemoveAllListeners();
        leftButton.onClick.RemoveAllListeners();
        rightButton.onClick.RemoveAllListeners();
        rotateButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();

        upButton.onClick.AddListener(() => currentPuzzle.MoveSelection("Up"));
        downButton.onClick.AddListener(() => currentPuzzle.MoveSelection("Down"));
        leftButton.onClick.AddListener(() => currentPuzzle.MoveSelection("Left"));
        rightButton.onClick.AddListener(() => currentPuzzle.MoveSelection("Right"));
        rotateButton.onClick.AddListener(() => currentPuzzle.RotateSelectedPiece());
        exitButton.onClick.AddListener(() => currentPuzzle.OnExit());
    }
    public override void Opened(params object[] param)
    {
        EditorLog.Log("실행");
        base.Opened(param);

        currentPuzzle.GeneratePuzzle();
        currentPuzzle.StartPuzzle();

    }
}
