using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerController controller;
    public PlayerController Controller { get { return controller; } }

    private PlayerFormController stateMachine;
    public PlayerFormController StateMachine { get { return stateMachine; } }

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        stateMachine = GetComponent<PlayerFormController>();

        stateMachine.PlayerSc = this;
    }
}
