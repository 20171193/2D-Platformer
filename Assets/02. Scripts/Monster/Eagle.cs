using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : Monster
{
    public enum EagleState{ Idle, Trace, Return, Die }

    [Header("FSM")]
    [SerializeField]
    private StateMachine<EagleState> fsm;

    [SerializeField]
    private IdleState<Eagle> idleState;
    [SerializeField]
    private TraceState<Eagle> traceState;
    [SerializeField]
    private ReturnState<Eagle> returnState;

    private void Awake()
    {
        fsm = new StateMachine<EagleState>();

        fsm.AddState(EagleState.Idle, idleState);
    }

    private void Start()
    {
        base.Initialize();
        fsm.SetInitState(EagleState.Idle);
    }

    private void Update()
    {
        fsm.Update();

        if (GetDistanceToPlayer() < detectRange)
            fsm.ChangeState(EagleState.Trace);
    }
}
