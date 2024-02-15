using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
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
    public Transform PlayerTransform { get { return playerTransform; } }

    [SerializeField]
    protected Vector3 startPos;
    public Vector3 StartPos { get { return startPos; } }

    [SerializeField]
    protected MonsterType monsterType;
    public MonsterType MonsterType { get { return monsterType; } }

    [SerializeField]
    protected float moveSpeed;
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }

    [SerializeField]
    protected float detectRange;
    public float DetectRange { get { return detectRange; } set { detectRange = value; } }

    [SerializeField]
    protected Vector3 moveDir;
    public Vector3 MoveDir { get { return moveDir; } set { moveDir = value; } }

    //[SerializeField]
    //protected MonsterState curState;

    protected void Initialize()
    {
        startPos = this.transform.position;
        playerTransform = GameObject.FindWithTag("Player").transform;

        //switch (monsterType)
        //{
        //    case MonsterType.Normal:
        //        break;
        //    case MonsterType.Tracker:
        //        break;
        //    default:
        //        break;
        //}
    }
    public float GetDistanceToPlayer()
    {
        return Vector2.Distance(transform.position, playerTransform.position);
    }
    public Vector2 GetDirectionToPlayer()
    {
        return (transform.position - playerTransform.position).normalized;
    }
}
