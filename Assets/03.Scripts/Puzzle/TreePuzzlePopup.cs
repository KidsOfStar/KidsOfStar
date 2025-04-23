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
    [SerializeField] private Button cancelButton;

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
        cancelButton.onClick.AddListener(() => OnCancelButtonClicked());

        // 팝업 닫기 버튼
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

        Managers.Instance.GameManager.Player.Controller.IsControllable = false;
        currentPuzzle.SetupPuzzle(data, index);
        currentPuzzle.GeneratePuzzle();
        currentPuzzle.StartPuzzle();
    }

    private void OnCancelButtonClicked()
    {
        currentPuzzle.StopPuzzle();
        Managers.Instance.UIManager.Hide<TreePuzzlePopup>();
        Managers.Instance.GameManager.Player.Controller.IsControllable = true;
    }
}
