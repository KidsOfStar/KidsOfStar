using System.Collections;
using UnityEngine;

public class Cobweb : MonoBehaviour
{
    public Collider2D cobwebCollider;
    public float slowDownFacto; // 느려지는 비율
    private float originalJumpForce; // 점프력

    private void Start()
    {
        cobwebCollider = GetComponent<Collider2D>();
        originalJumpForce = Managers.Instance.GameManager.Player.Controller.JumpForce;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") &&
            other.TryGetComponent(out PlayerController playerController) &&
            other.TryGetComponent(out PlayerFormController formController))
        {
            {
            // 플레이어의 속도를 0으로 설정
            playerController.MoveSpeed /= slowDownFacto;

            // 플레이어의 점프력을 0으로 설정
            playerController.JumpForce = 0f;
            }

            if (formController.CurFormData.playerFormType == PlayerFormType.Dog)
            {
                StartCoroutine(DogBreakCobweb(playerController));
            }
        }
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") 
            && other.TryGetComponent(out PlayerController playerController)
            && other.TryGetComponent(out PlayerFormController formController))
        {
            // 만약 변신 등의 이유로 점프력이 다시 설정되었다면,
            // 거미줄 안에 있는 동안은 강제로 0으로 유지
            if (playerController.JumpForce != 0f)
            {
                playerController.JumpForce = 0f;
            }

            if (formController.CurFormData.playerFormType == PlayerFormType.Dog)
            {
                StartCoroutine(DogBreakCobweb(playerController));
            }
        }
    }

    private IEnumerator DogBreakCobweb(PlayerController playerController)
    {
        yield return new WaitForSeconds(1f);

        playerController.JumpForce = originalJumpForce;

        Destroy(gameObject);
        Managers.Instance.SoundManager.PlaySfx(SfxSoundType.WallBreak);
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.gameObject.TryGetComponent(out PlayerController playerController))
        {
            // 플레이어의  속도를 원래대로 복원
            playerController.MoveSpeed *= slowDownFacto;

            // 플레이어의 점프력을 0으로 설정
            playerController.JumpForce = originalJumpForce;
        }
    }
}