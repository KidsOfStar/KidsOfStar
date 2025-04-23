using UnityEngine;
using UnityEngine.UI;

public class TreePuzzlePopup : PopupBase
{
    [SerializeField] private Button upButton;
    [SerializeField] private Button downButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button rotateButton;
    [SerializeField] private Button[] exitButtons;

    [SerializeField] private TreePuzzleSystem currentPuzzle;

    private void Awake()
    {
        upButton.onClick.RemoveAllListeners();
        downButton.onClick.RemoveAllListeners();
        leftButton.onClick.RemoveAllListeners();
        rightButton.onClick.RemoveAllListeners();
        rotateButton.onClick.RemoveAllListeners();       

        upButton.onClick.AddListener(() => currentPuzzle.MoveSelection("Up"));
        downButton.onClick.AddListener(() => currentPuzzle.MoveSelection("Down"));
        leftButton.onClick.AddListener(() => currentPuzzle.MoveSelection("Left"));
        rightButton.onClick.AddListener(() => currentPuzzle.MoveSelection("Right"));
        rotateButton.onClick.AddListener(() => currentPuzzle.RotateSelectedPiece());

        foreach (var btn in exitButtons)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => currentPuzzle.OnExit());
        }
    }
    public override void Opened(params object[] param)
    {
        EditorLog.Log("실행");
        base.Opened(param);

        var data = param[0] as TreePuzzleData;
        if (data == null)
        {
            EditorLog.LogError("TreePuzzlePopup: PuzzleData 누락!");
            return;
        }

        int index = (int)param[1];

        currentPuzzle.SetupPuzzle(data, index);
        currentPuzzle.GeneratePuzzle();
        currentPuzzle.StartPuzzle();
    }
}
