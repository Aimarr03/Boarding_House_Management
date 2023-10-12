using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay_StateDefault
{
    protected GameplayScene _scene;
    protected Gameplay_StateMachine _stateMachine;
    public Gameplay_StateDefault(GameplayScene scene, Gameplay_StateMachine stateMachine)
    {
        _scene = scene;
        _stateMachine = stateMachine;
    }
    public virtual void EnteringState()
    {

    }
    public virtual void Hovering()
    {

    }
    public virtual void OnClicked()
    {

    }
    public virtual void ExitingState()
    {

    }
}
