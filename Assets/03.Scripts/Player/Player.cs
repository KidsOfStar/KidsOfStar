using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerController controller;
    public PlayerController Controller { get { return controller; } }

    private PlayerFormController formControl;
    public PlayerFormController FormControl { get { return formControl; } }

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        formControl = GetComponent<PlayerFormController>();

        controller.PlayerSc = this;
        formControl.PlayerSc = this;
    }
}