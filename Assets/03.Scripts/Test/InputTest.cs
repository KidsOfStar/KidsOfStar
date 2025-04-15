using UnityEngine;

public class InputTest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Managers.Instance.DialogueManager.OnClick?.Invoke();
        }
    }
}
