
using UnityEngine;

public interface IPuzzleTrigger
{
    int SequenceIndex { get; }
    void ResetTrigger();
    void DisableExclamation();
}

public abstract class PuzzleTriggerBase : MonoBehaviour, IPuzzleTrigger
{
    [SerializeField] protected int sequenceIndex;
    public int SequenceIndex => sequenceIndex;

    [SerializeField] protected int requiredProgress;

    [SerializeField] protected GameObject exclamationInstance;
    protected SpriteRenderer exclamationRenderer;

    protected bool triggered = false;
    protected bool tutorialShown = false;

    protected SkillBTN skillBtn;

    public virtual void InitTrigger()
    {
        if (exclamationInstance != null)
            exclamationRenderer = exclamationInstance.GetComponent<SpriteRenderer>();

        Managers.Instance.PuzzleManager.RegisterTrigger(this);
    }

    public void SetupUI()
    {
        skillBtn = Managers.Instance.UIManager.Get<PlayerBtn>().skillPanel;
        Managers.Instance.GameManager.OnProgressUpdated += UpdateExclamation;

        UpdateExclamation();
    }

    public void Cleanup()
    {
        Managers.Instance.GameManager.OnProgressUpdated -= UpdateExclamation;
        Managers.Instance.PuzzleManager.UnregisterTrigger(this);
    }

    protected virtual void UpdateExclamation()
    {
        bool show = Managers.Instance.GameManager.ChapterProgress == requiredProgress;
        if (exclamationRenderer != null)
            exclamationRenderer.enabled = show;
    }

    public virtual void DisableExclamation()
    {
        if (exclamationRenderer != null)
            exclamationRenderer.enabled = false;
    }

    public abstract void ResetTrigger(); // 추상 메서드로 자식에서 구현

    protected void HideInteraction()
    {
        skillBtn.ShowInteractionButton(false);
        skillBtn.OnInteractBtnClick -= OnPuzzleButtonPressed;
    }

    protected void SetupInteraction()
    {
        skillBtn.ShowInteractionButton(true);
        skillBtn.OnInteractBtnClick -= OnPuzzleButtonPressed;
        skillBtn.OnInteractBtnClick += OnPuzzleButtonPressed;
    }

    protected abstract void OnPuzzleButtonPressed();

    protected virtual void OnDestroy()
    {
        Cleanup();
    }
}

