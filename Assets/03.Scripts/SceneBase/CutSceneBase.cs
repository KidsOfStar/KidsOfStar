using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutSceneBase : MonoBehaviour
{
    [field: SerializeField] public PlayableDirector Director { get; private set; }
    [field: SerializeField] private SceneType sceneType;
    [SerializeField] private SignalReceiver signalReceiver;
    [SerializeField] private SignalAsset dialogSignal;
    [SerializeField] private SignalAsset destroySignal;

    [Header("CutSceneData")]
    [SerializeField] private CutSceneData cutSceneData;

    public Action OnCutSceneCompleted { get; set; }
    private int currentIndex;

    public void Init()
    {
        if (cutSceneData.Npcs != null)
        {
            Managers.Instance.DialogueManager.InitCutSceneNPcs(cutSceneData.Npcs);
        }

        SwapCameraInTimeline();
        Managers.Instance.DialogueManager.OnDialogEnd += ResumeCutScene;
    }

    public void SwapCameraInTimeline()
    {
        var timeline = Director.playableAsset as TimelineAsset;
        if (!timeline)
        {
            Debug.LogError("CutSceneBase : TimelineAsset is null");
            return;
        }

        var brain = Managers.Instance.GameManager.MainCamera.GetComponent<CinemachineBrain>();
        foreach (var track in timeline.GetOutputTracks())
        {
            if (track is CinemachineTrack cinemachineTrack)
                Director.SetGenericBinding(cinemachineTrack, brain);
        }
    }

    public void Play()
    {
        Director.Play();
    }

    public void ShowDialog()
    {
        //director.Evaluate();
        //director.Pause();
        Director.playableGraph.GetRootPlayable(0).SetSpeed(0);

        var index = cutSceneData.DialogIndexes[currentIndex];
        Managers.Instance.DialogueManager.SetCurrentDialogData(index);
        currentIndex++;
    }

    private void ResumeCutScene()
    {
        Director.playableGraph.GetRootPlayable(0).SetSpeed(1);
        Director.Resume();
    }

    public void DestroyPrefab()
    {
        //Managers.Instance.CutSceneManager.LetterBoxer.DisableLetterBox();
        Managers.Instance.DialogueManager.OnDialogEnd -= ResumeCutScene;
        Managers.Instance.CutSceneManager.OnCutSceneEnd?.Invoke();
        OnCutSceneCompleted?.Invoke();
        if (sceneType != SceneType.Title)
            Managers.Instance.SceneLoadManager.LoadScene(sceneType);
        
        Destroy(gameObject);
    }
}