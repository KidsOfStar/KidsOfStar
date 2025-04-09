using UnityEngine;

public class DialogInputHandler : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Managers.Instance.DialogueManager.OnClick?.Invoke();
        }
    }
}
