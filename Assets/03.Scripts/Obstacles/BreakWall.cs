using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakWall : MonoBehaviour
{
    // 벽을 부술 수 있는 오브젝트
    // 벽을 부술 수 있는 오브젝트는 Player의 형태변화 강아지

    public Collider2D wallCollider;

    public Player player;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerFormController playerFormController))
        {
            if (playerFormController.CurFormData.FormName == "Dog")
            {
                Destroy(gameObject);
            }
        }
    }
}

