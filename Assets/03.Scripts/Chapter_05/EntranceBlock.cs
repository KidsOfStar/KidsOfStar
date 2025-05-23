using UnityEngine;

public class EntranceBlock : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Player>();
            if (player != null)
            {
                var formType = player.FormControl.CurFormData.playerFormType;

                // 은신 폼(Hide)이 아닐 경우 무조건 막기
                if ((formType == PlayerFormType.Hide))
                {
                    player.Controller.IsControllable = false;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Player>();
            if (player != null)
            {
                // 이동 가능 복구
                player.Controller.IsControllable = true;
            }
        }
    }
}
