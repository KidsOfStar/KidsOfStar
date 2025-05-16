using System.Collections.Generic;
using UnityEngine;

public class ObjectIndicator : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] Box box;

    [Header("Object가 켜질 챕터 진행도")]
    [SerializeField] int progress;

    [Header("리셋 위치")]
    [Tooltip("리셋 시 플레이어가 이동할 위치")]
    [SerializeField] private Vector3 resetPlayerPos;
    [Tooltip("리셋 시 박스가 이동할 위치")]
    [SerializeField] private Vector3 resetBoxPos;

    public void Start()
    {
        player = Managers.Instance.GameManager.Player.Controller;
        UpdateActive();
        Managers.Instance.GameManager.OnProgressUpdated += UpdateActive;
    }
    void OnDestroy()
    {
        Managers.Instance.GameManager.OnProgressUpdated -= UpdateActive;
    }

    private void UpdateActive()
    {
        bool isActive = Managers.Instance.GameManager.ChapterProgress >= progress;
        gameObject.SetActive(isActive);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!gameObject.activeSelf) return;

        if (Managers.Instance.GameManager.CurrentChapter == ChapterType.Chapter2 
            && other.gameObject.CompareTag("Box"))
        {
            Managers.Instance.UIManager.Show<TreeWarningPopup>(
                WarningType.BoxFalling, this);
        }
        Time.timeScale = 0f;
    }

    public void ResetPosition()
    {
        player.playerBasePos = resetPlayerPos;
        player.ResetPlayer();

        box.boxBasePos = resetBoxPos;
        box.ResetPosition();

        Time.timeScale = 1f;

        if(Managers.Instance.GameManager.CurrentChapter== ChapterType.Chapter2)
        Managers.Instance.UIManager.Hide<TreeWarningPopup>();
    }
}
