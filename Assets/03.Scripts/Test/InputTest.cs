using UnityEngine;

public class InputTest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Managers.Instance.DialogueManager.OnClick?.Invoke();
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Managers.Instance.CutSceneManager.PlayCutScene(CutSceneType.Chapter02_Test.GetName());
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            Managers.Instance.DialogueManager.SetCurrentDialogData(10006);
        }
    }
}
