using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerController controller;
    public PlayerController Controller { get { return controller; } }

    private PlayerFormController formControl;
    public PlayerFormController FormControl { get { return formControl; } }

    private PlayerStateMachine stateMachine;
    public PlayerStateMachine StateMachine { get { return stateMachine; } }

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        formControl = GetComponent<PlayerFormController>();
        stateMachine = GetComponent<PlayerStateMachine>();

        formControl.PlayerSc = this;
        stateMachine.Player = this;

        controller.Init(this);

    }

    private void Start()
    {
        Managers.Instance.GameManager.SetPlayer(this);
    }
}