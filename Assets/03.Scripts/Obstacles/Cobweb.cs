using UnityEngine;

public class Cobweb : MonoBehaviour
{
    public Collider2D cobwebCollider;
    public float slowDownFacto; // 느려지는 비율
    public float originalJumpForce; // 점프력

    private void Start()
    {
        cobwebCollider = GetComponent<Collider2D>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        EditorLog.Log("OnTriggerEnter2D");
        if (other.CompareTag("Player") && other.gameObject.TryGetComponent(out PlayerController playerController))
        {
            // 플레이어의 속도를 0으로 설정
            playerController.MoveSpeed /= slowDownFacto;
            // 원래 점프력 저장
            originalJumpForce = playerController.JumpForce;

            // 플레이어의 점프력을 0으로 설정
            playerController.JumpForce = 0f;
        }
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        EditorLog.Log("OnTriggerStay2D");
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerController playerController))
        {
            // 만약 변신 등의 이유로 점프력이 다시 설정되었다면,
            // 거미줄 안에 있는 동안은 강제로 0으로 유지
            if (playerController.JumpForce != 0f)
            {
                playerController.JumpForce = 0f;
            }
        }
    }



    public void OnTriggerExit2D(Collider2D other)
    {
        EditorLog.Log("OnTriggerExit2D");
        if (other.CompareTag("Player") && other.gameObject.TryGetComponent(out PlayerController playerController))
        {
            // 플레이어의 속도를 0으로 설정
            playerController.MoveSpeed *= slowDownFacto;

            // 플레이어의 점프력을 0으로 설정
            playerController.JumpForce = originalJumpForce;
        }
    }
    

}


