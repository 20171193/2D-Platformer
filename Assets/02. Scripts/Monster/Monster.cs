using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    Normal,
    Tracker
}

public class Monster : MonoBehaviour
{
    [SerializeField]
    protected Transform playerTransform;

    [SerializeField]
    protected Vector3 startPos;

    [SerializeField]
    protected MonsterType monsterType;
    //[SerializeField]
    //protected MonsterState curState;

    protected void Initialize()
    {
        //curState = MonsterState.Idle;
        startPos = this.transform.position;

        switch (monsterType)
        {
            case MonsterType.Normal:
                break;
            case MonsterType.Tracker:
                playerTransform = GameObject.FindWithTag("Player").transform;
                break;
            default:
                break;
        }
    }

}
