using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public IState curState;

    public void Update()
    {
        curState.Update();
    }
    public void ChangeState(IState nextState)
    {
        curState.Exit();
        curState = nextState;
        curState.Enter();
    }
}
