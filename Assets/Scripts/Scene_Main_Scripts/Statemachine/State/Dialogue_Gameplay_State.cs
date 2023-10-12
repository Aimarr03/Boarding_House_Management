using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue_Gameplay_State : Gameplay_StateDefault
{
    public Dialogue_Gameplay_State(GameplayScene scene, Gameplay_StateMachine stateMachine) : base(scene, stateMachine)
    {
    }
    public override void EnteringState()
    {
        base.EnteringState();
    }

    public override void Hovering()
    {
        base.Hovering();
    }

    public override void OnClicked()
    {
        base.OnClicked();
    }

    public override void ExitingState()
    {
        base.ExitingState();
    }
}
