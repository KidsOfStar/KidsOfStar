using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerController controller;
    public PlayerController Controller { get { return controller; } }

    private PlayerFormController formControl;
    public PlayerFormController FormControl { get { return formControl; } }

    public PlayerBtn playerBtn; // PlayerBtn 참조

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        formControl = GetComponent<PlayerFormController>();

        formControl.PlayerSc = this;
    }

    private void Start()
    {
        Managers.Instance.GameManager.SetPlayer(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Interactable"))
        {
            playerBtn.ShowInteractionButton(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            playerBtn.ShowInteractionButton(false);
        }
    }

}
