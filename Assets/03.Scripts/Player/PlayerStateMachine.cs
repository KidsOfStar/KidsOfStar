using System;
using System.Collections;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private Player player;
    public Player Player { set {  player = value; } }
    private IPlayerState curState;
    public IPlayerState CurState { get { return curState; } }
    [SerializeField] private PlayerContextData contextData;
    public PlayerContextData ContextData { get { return contextData; } }
    private PlayerStateFactory factory;
    public PlayerStateFactory Factory {  get { return factory; } }

    public void Init(Player player)
    {
        this.player = player;
        contextData = new PlayerContextData(player, player.Controller, player.FormControl, this,
            GetComponentInChildren<SpriteRenderer>(), GetComponent<Rigidbody2D>(), GetComponent<BoxCollider2D>());
        factory = new PlayerStateFactory(contextData);
        ChangeState(factory.GetPlayerState(PlayerStateType.Idle));
    }

    void Update()
    {
        if (!player.Controller.IsControllable) return;

        curState?.OnUpdate();
    }

    public void ChangeState(IPlayerState nextState)
    {
        if (curState == nextState) return;
        curState?.OnExit();
        curState = nextState;
        curState?.OnEnter();
    }

    public void ReplaceStartCoroutine(float delay, Action action)
    {
        StartCoroutine(InvokeCoroutine(delay, action));
    }

    private IEnumerator InvokeCoroutine(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }
}
