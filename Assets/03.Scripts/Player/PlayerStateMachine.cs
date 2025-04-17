using System;
using System.Collections;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private Player player;
    public Player Player { set {  player = value; } }
    private IPlayerState curState;
    public IPlayerState CurState { get { return curState; } }
    private PlayerContextData contextData;
    private PlayerStateFactory factory;
    public PlayerStateFactory Factory {  get { return factory; } }

    private void Start()
    {
        contextData = new PlayerContextData(player, player.Controller, player.FormControl, this,
            GetComponent<SpriteRenderer>(), GetComponent<Rigidbody2D>(), GetComponent<BoxCollider2D>());
        factory = new PlayerStateFactory(contextData);
        ChangeState(factory.GetPlayerState(PlayerStateType.Idle));
    }

    void Update()
    {
        curState?.OnUpdate();
    }

    public void ChangeState(IPlayerState nextState)
    {
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
