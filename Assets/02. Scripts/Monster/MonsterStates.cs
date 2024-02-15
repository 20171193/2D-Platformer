using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region MonsterState Base Class
public class MonsterState<T> where T : Monster
{
    [SerializeField]
    protected T owner;
}
#endregion

#region Monster States
public class IdleState<T> : MonsterState<T>, IState where T : Monster
{
    public void Enter()
    {
    }
    public void Update()
    {
    }
    public void Exit()
    {
    }
}

public class TraceState<T> : IState where T : Monster
{
    public void Enter()
    {
    }
    public void Update()
    {
    }
    public void Exit()
    {
    }
}

public class ReturnState<T> : IState where T : Monster
{
    public void Enter()
    {

    }
    public void Update()
    {

    }
    public void Exit()
    {

    }
}

public class DieState<T> : IState where T : Monster
{
    public void Enter()
    {

    }
    public void Update()
    {

    }
    public void Exit()
    {

    }
}

#endregion
