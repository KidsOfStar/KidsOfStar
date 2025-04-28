using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ObjectIndicator : MonoBehaviour
{
    [SerializeField] GameObject box;
    [SerializeField] Player player;

    public void Start()
    {
        player = Managers.Instance.GameManager.Player;
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
        player.transform.position = new Vector3(-6.5f, 2.6f, -10f);
        box.transform.position = new Vector3(-5.1f, 2.9f, 0f);
        Time.timeScale = 1f;
        Managers.Instance.UIManager.Hide<TreeWarningPopup>();
    }
}
