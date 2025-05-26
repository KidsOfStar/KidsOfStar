using UnityEngine;

public class TreePuzzleTrigger : PuzzleTriggerBase
{ 

    [Header("튜토리얼 문인지 체크")]
    [SerializeField] private bool isTutorialDoor = false;
    
    [SerializeField] private TreePuzzleData puzzleData;

    [SerializeField] private string puzzleLayerName = "PuzzleDoor";
    private int puzzleLayer;

    public override void InitTrigger()
    {
        base.InitTrigger();
        puzzleLayer = LayerMask.NameToLayer(puzzleLayerName);

        if (gameObject.layer != puzzleLayer)
        {
            enabled = false;
        }
        Managers.Instance.PuzzleManager.RegisterTrigger(PuzzleId, this);
    }

    public override void ResetTrigger()
    {
        base.ResetTrigger();

        if (hasPlayer && hasBox)
        {
            SetupInteraction();
        }
    }
    protected override void OnPuzzleButtonPressed()
    {
        Managers.Instance.SoundManager.PlaySfx(SfxSoundType.Communication);
        TryStartPuzzle();
    }

    protected override void TryStartPuzzle()
    {
        base.TryStartPuzzle();

        if (isTutorialDoor &&
            requiredProgress == Managers.Instance.GameManager.ChapterProgress &&
            sequenceIndex == 0 &&
            !tutorialShown)
        {
            tutorialShown = true;
            var popup = Managers.Instance.UIManager.Show<TutorialPopup>(0);
            popup.OnClosed += () =>
            {
                Managers.Instance.UIManager.Show<TreePuzzlePopup>(puzzleData, sequenceIndex);
            };
            return;
        }

        Managers.Instance.UIManager.Show<TreePuzzlePopup>(puzzleData, sequenceIndex);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (triggered || Managers.Instance.GameManager.ChapterProgress != requiredProgress)
            return;

        if (collision.CompareTag("Player"))
        {
            // 다람쥐 형태 검사
            var formControl = Managers.Instance.GameManager.Player.FormControl;
            bool isSquirrel = formControl.ReturnCurFormType() == PlayerFormType.Squirrel;

            if (isSquirrel && !hasBox)
            {
                // 다람쥐 & 상자 없음 → 두 문구를 순차적으로
                Managers.Instance.UIManager.Show<WarningPopup>(
                    WarningType.Squirrel,
                    WarningType.BoxMissing
                );
                return;
            }
            else if (isSquirrel)
            {
                // 다람쥐지만 상자는 들고 있는 경우 → 한 문구만
                Managers.Instance.UIManager.Show<WarningPopup>(
                    WarningType.Squirrel
                );
                return;
            }

            if (!hasBox)
            {
                // 다람쥐가 아니지만, 상자가 없는 경우
                Managers.Instance.UIManager.Show<WarningPopup>(
                    WarningType.BoxMissing
                );
            }
        }
    }
}
