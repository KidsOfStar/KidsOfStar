using UnityEngine;

public class ObjectIndicator : MonoBehaviour
{
    [SerializeField] GameObject box;
    [SerializeField] Player player;

    private Rigidbody2D rb;
    public void Start()
    {
        player = Managers.Instance.GameManager.Player;
        rb = box.gameObject.GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Box"))
        {
            rb.velocity = Vector2.zero;
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
