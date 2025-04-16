using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateBase : IPlayerState
{
    protected PlayerContextData context;
    protected PlayerStateFactory factory;

    public PlayerStateBase(PlayerContextData data, PlayerStateFactory factory)
    {
        this.context = data;
        this.factory = factory;
    }

    public virtual void OnEnter()
    {
        
    }

    public virtual void OnExit()
    {
        
    }

    public virtual void OnUpdate()
    {
        
    }
}
