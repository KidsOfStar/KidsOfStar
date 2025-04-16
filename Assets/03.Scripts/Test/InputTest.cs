using UnityEngine;

public class InputTest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Managers.Instance.DialogueManager.OnClick?.Invoke();
        }
        
        if (Input.GetKeyDown(KeyCode.H))
        {
            Managers.Instance.SceneLoadManager.LoadScene(SceneType.Title);
        }
    }
}
