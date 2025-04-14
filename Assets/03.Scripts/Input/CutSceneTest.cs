using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutSceneTest : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    [SerializeField] private SignalReceiver signalReceiver;
    [SerializeField] private SignalAsset dialogSignal;
    
    [Header("CutSceneData")]
    [SerializeField] private CutSceneData cutSceneData;

    private UnityEvent showDialogEvent;
    private int currentIndex = 0;

    private void Start()
    {
        Managers.Instance.DialogueManager.InitCutSceneNPcs(cutSceneData.Npcs);
        Managers.Instance.DialogueManager.OnDialogEnd += ResumeCutScene;
        
        showDialogEvent.AddListener(ShowDialog);
        signalReceiver.AddReaction(dialogSignal, showDialogEvent);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentIndex = 0;
            director.Play();
        }
    }
    
    public void ShowDialog()
    {
        director.Pause();
        var index = cutSceneData.DialogIndexes[currentIndex];
        Managers.Instance.DialogueManager.SetCurrentDialogData(index);
        currentIndex++;
    }
    
    private void ResumeCutScene()
    {
        director.Resume();
    }
}