using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutSceneBase : MonoBehaviour
{
    [field: SerializeField] public PlayableDirector Director { get; private set; }
    [SerializeField] private SignalReceiver signalReceiver;
    [SerializeField] private SignalAsset destroySignal;

    public Action OnCutSceneCompleted { get; set; }
    private int currentIndex;

    public void Init()
    {
        if (TryGetComponent(out DialogPlayer dialogPlayer))
        {
            Managers.Instance.DialogueManager.InitCutSceneNPcs(dialogPlayer.Npcs);
        }

        SetCinemachineBrain();
        Managers.Instance.DialogueManager.OnDialogEnd += ResumeCutScene;
    }

    private void SetCinemachineBrain()
    {
        var timeline = Director.playableAsset as TimelineAsset;
        if (!timeline)
        {
            EditorLog.LogError("CutSceneBase : TimelineAsset is null");
            return;
        }

        if (Managers.Instance.GameManager.MainCamera.TryGetComponent(out CinemachineBrain brain))
        {
            foreach (var track in timeline.GetOutputTracks())
            {
                if (track is CinemachineTrack cinemachineTrack)

                    Director.SetGenericBinding(cinemachineTrack, brain);
            }
        }
        else
        {
            EditorLog.LogError("CutSceneBase : MainCamera does not have a CinemachineBrain component");
        }
    }

    public void Play()
    {
        Director.Play();
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
        
        if (TryGetComponent(out SceneLoadable sceneLoadable)) sceneLoadable.LoadScene();
        Destroy(gameObject);
    }
}