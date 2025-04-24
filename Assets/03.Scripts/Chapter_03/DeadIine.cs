using UnityEngine;

public class DeadIine : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.gameObject.SetActive(false);
                Managers.Instance.GameManager.TriggerEnding(EndingType.Mistake);
            }
        }
    }
}
