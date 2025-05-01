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
        // 퍼즐선택버튼
        var moveButtons = new (Button btn, System.Action action)[]
        {
            (upButton, () => currentPuzzle.MoveSelection("Up")),
            (downButton, ()=> currentPuzzle.MoveSelection("Down")),
            (leftButton, () => currentPuzzle.MoveSelection("Left")),
            (rightButton, () => currentPuzzle.MoveSelection("Right")),
         };

        foreach (var (btn, action) in moveButtons)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                Managers.Instance.SoundManager.PlaySfx(SfxSoundType.UIButton);
                action();
            });
        }

        // 회전버튼
        rotateButton.onClick.RemoveAllListeners();
        rotateButton.onClick.AddListener(() =>
        {
            Managers.Instance.SoundManager.PlaySfx(SfxSoundType.TurnPuzzle);
            currentPuzzle.RotateSelectedPiece();
        });

        // 팝업 닫기버튼
        cancelButton.onClick.AddListener(() =>
        {
            Managers.Instance.SoundManager.PlaySfx(SfxSoundType.UICancel);
            OnCancelButtonClicked();
        });

        // 팝업 닫기 버튼
        foreach (var btn in exitButtons)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                Managers.Instance.SoundManager.PlaySfx(SfxSoundType.UICancel);
                OnCancelButtonClicked();
            });
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
