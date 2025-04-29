using UnityEngine;

public class Player : MonoBehaviour, IDialogSpeaker
{
    private PlayerController controller;
    public PlayerController Controller { get { return controller; } }

    private PlayerFormController formControl;
    public PlayerFormController FormControl { get { return formControl; } }

    private PlayerStateMachine stateMachine;
    public PlayerStateMachine StateMachine { get { return stateMachine; } }

    [SerializeField, Tooltip("말풍선 위치")] private Transform bubblePosition;

    public void Init(string formName)
    {
        controller = GetComponent<PlayerController>();
        formControl = GetComponent<PlayerFormController>();
        stateMachine = GetComponent<PlayerStateMachine>();

        controller.Init(this);
        formControl.Init(this, formName);
        stateMachine.Init(this);

        formControl.CutSceneFormChange(formName == "" ? "Human" : formName);
    }

    public CharacterType GetCharacterType()
    {
        return CharacterType.Dolmengee;
    }

    public Vector3 GetBubblePosition()
    {
        return bubblePosition.position;
    }
}