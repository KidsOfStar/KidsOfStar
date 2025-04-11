using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutSceneTest : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    [SerializeField] private CutSceneData cutSceneData;

    private int currentIndex = 0;

    private void Start()
    {
        Managers.Instance.DialogueManager.InitCutSceneNPcs(cutSceneData.Npcs);
        Managers.Instance.DialogueManager.OnDialogEnd += ResumeCutScene;
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
