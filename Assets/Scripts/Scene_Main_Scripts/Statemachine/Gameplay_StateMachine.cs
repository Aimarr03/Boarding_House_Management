using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay_StateMachine
{
    private Gameplay_StateDefault _currentGameplayState;
    
    public void Initialize(Gameplay_StateDefault input)
    {
        _currentGameplayState = input;
        _currentGameplayState.EnteringState();
    }
    public void ChangeState(Gameplay_StateDefault input)
    {
        _currentGameplayState.ExitingState();
        _currentGameplayState = input;
        _currentGameplayState.EnteringState();
    }
    public Gameplay_StateDefault GetState()
    {
        return _currentGameplayState;
    }
}
