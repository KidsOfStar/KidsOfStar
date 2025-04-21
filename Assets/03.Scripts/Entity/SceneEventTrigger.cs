using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneEventTrigger : MonoBehaviour
{
    [field: SerializeField] public UnityEvent OnTriggerEnterEvent { get; private set; }
    [field: SerializeField] private int[] requiredDialogs;
    private Dictionary<int, bool> finishedDialog = new();
    private bool canTrigger = false;
    private bool isDialogFinished = false;

    public void Init()
    {
        // 딕셔너리 초기화
        for (int i = 0; i < requiredDialogs.Length; i++)
        {
            var index = requiredDialogs[i];
            finishedDialog[index] = false;
        }
        
        // 콜백에 등록
        Managers.Instance.DialogueManager.OnSceneDialogEnd += CheckCurrentDialog;
    }

    private void CheckCurrentDialog(int index)
    {
        EditorLog.Log(index);
        
        if (finishedDialog.ContainsKey(index))
            finishedDialog[index] = true;
        
        CheckRequiredDialogFinished();
    }

    private void CheckRequiredDialogFinished()
    {
        // 하나라도 안본 대사가 있다면 return
        foreach (var value in finishedDialog.Values)
            if (!value) return;

        // 모두 봤다면 Trigger 가능
        canTrigger = true;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canTrigger) return;
        if (!other.CompareTag("Player")) return;

        OnTriggerEnterEvent?.Invoke();
        canTrigger = false;
    }

    private void OnDestroy()
    {
        Managers.Instance.DialogueManager.OnSceneDialogEnd -= CheckCurrentDialog;
    }
}
