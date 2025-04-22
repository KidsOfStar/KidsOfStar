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
                // 플레이어가 떨어졌을 때 타이틀 씬으로 이동
                Managers.Instance.SceneLoadManager.LoadScene(SceneType.Title);
            }
        }
    }
}
