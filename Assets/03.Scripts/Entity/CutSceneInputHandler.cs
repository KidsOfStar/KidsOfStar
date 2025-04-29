using UnityEngine;
using UnityEngine.EventSystems;

public class CutSceneInputHandler : MonoBehaviour
{
    private bool isTalk;

    private void Start()
    {
        Managers.Instance.DialogueManager.OnDialogStart += IsTalk;
        Managers.Instance.DialogueManager.OnDialogEnd += IsNotTalk;
    }

    private void Update()
    {
        if (!isTalk) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            
            Managers.Instance.DialogueManager.OnClick?.Invoke();
        }

        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P))
        {
            Managers.Instance.CutSceneManager.DestroyCurrentCutScene();
        }
        #endif
    }

    private void IsTalk()
    {
        isTalk = true;
    }

    private void IsNotTalk()
    {
        isTalk = false;
    }

    private void OnDestroy()
    {
        Managers.Instance.DialogueManager.OnDialogStart -= IsTalk;
        Managers.Instance.DialogueManager.OnDialogEnd -= IsNotTalk;
    }
}
