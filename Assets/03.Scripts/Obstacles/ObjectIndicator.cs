using UnityEngine;

public class ObjectIndicator : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] Box box;
    public void Start()
    {
        player = Managers.Instance.GameManager.Player.Controller;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Box"))
        {
            Managers.Instance.UIManager.Show<TreeWarningPopup>(
                WarningType.BoxFalling);
            Time.timeScale = 0f;
        }
    }

    public void ResetPosition()
    {
        player.ResetPlayer();
        box.ResetPosition();
        Time.timeScale = 1f;
        Managers.Instance.UIManager.Hide<TreeWarningPopup>();
        
    }
}
