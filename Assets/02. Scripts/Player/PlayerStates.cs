using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class PlayerStateBase
{
    [SerializeField]
    protected PlayerController owner;
}

#region Player States
public class IdleState : PlayerStateBase, IState 
{
    public IdleState(PlayerController owner)
    {
        this.owner = owner;
    }

    public void Enter()
    {
        owner.CurState = PlayerState.Idle;
        owner.Animator.Play("Idle");
    }
    public void Update()
    {
        
    }
    public void Exit()
    {

    }
}
public class RunState : PlayerStateBase, IState
{
    public RunState(PlayerController owner)
    {
        this.owner = owner;
    }

    public void Enter()
    {
        owner.CurState = PlayerState.Run;
        owner.Animator.Play("Run");
    }
    public void Update()
    {

    }
    public void Exit()
    {

    }
}
public class JumpState : PlayerStateBase, IState
{
    public JumpState(PlayerController owner)
    {
        this.owner = owner;
    }
    public void Enter()
    {
        owner.CurState = PlayerState.Jump;
        owner.Animator.Play("Jump");
    }
    public void Update()
    {

    }
    public void Exit()
    {

    }
}
public class FallState : PlayerStateBase, IState
{
    public FallState(PlayerController owner)
    { 
        this.owner = owner;
    }
    public void Enter()
    {
        owner.CurState = PlayerState.Fall;
        owner.Animator.Play("Fall");
    }
    public void Update()
    {

    }
    public void Exit()
    {

    }
}
public class CrouchState : PlayerStateBase, IState
{
    public CrouchState(PlayerController owner)
    {
        this.owner = owner;
    }
    public void Enter()
    {
        owner.CurState = PlayerState.Crouch;
        owner.Animator.Play("Crouch");
    }
    public void Update()
    {

    }
    public void Exit()
    {

    }
}
public class ClimbIdleState : PlayerStateBase, IState
{
    public ClimbIdleState(PlayerController owner)
    {
        this.owner = owner;
    }
    public void Enter()
    {
        owner.CurState = PlayerState.ClimbIdle;
        owner.Animator.Play("ClimbIdle");

    }
    public void Update()
    {

    }
    public void Exit()
    {

    }
}
public class ClimbingState : PlayerStateBase, IState
{
    public ClimbingState(PlayerController owner)
    {
        this.owner = owner;
    }
    public void Enter()
    {
        owner.CurState = PlayerState.Climbing;
        owner.Animator.Play("Climbing");
    }
    public void Update()
    {

    }
    public void Exit()
    {

    }
}
public class DieState : PlayerStateBase, IState
{
    public DieState(PlayerController owner)
    {
        this.owner = owner;
    }
    public void Enter()
    {
        owner.CurState = PlayerState.Die;
        owner.Animator.Play("Die");
    }
    public void Update()
    {

    }
    public void Exit()
    {

    }
}

#endregion
