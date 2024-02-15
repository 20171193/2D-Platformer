using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : Monster
{
    [SerializeField]
    float moveSpeed;

    [SerializeField]
    float detectRange;

    [SerializeField]
    Vector3 moveDir;

    private void Start()
    {
        base.Initialize();
    }

    private void Update()
    {

    }

    //public void ChangeState(GameObject agent, MonsterState nextState)
    //{

    //}

    private void IdleUpdate()
    {
        if(Vector2.Distance(playerTransform.position, transform.position) < detectRange)
        {
        }
    }
    private void TraceUpdate()
    {

    }
    private void ReturnUpdate()
    {

    }

}
