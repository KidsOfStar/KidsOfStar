using UnityEngine;
using UnityEngine.Events;

public class SceneEventTrigger : MonoBehaviour
{
    [field: SerializeField] public UnityEvent OnTriggerEnterEvent { get; private set; }
    private bool isTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isTriggered) return;
        if (!other.CompareTag("Player")) return;

        OnTriggerEnterEvent?.Invoke();
        isTriggered = true;
    }
}
