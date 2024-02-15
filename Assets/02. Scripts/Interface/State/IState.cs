using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Idle,
    Die
}

public interface IState
{
    public void Enter();
    public void Update();
    public void Exit();
    void ChangeState(State nextState);
    void IdleUpdate();
    void DieUpdate();
}
