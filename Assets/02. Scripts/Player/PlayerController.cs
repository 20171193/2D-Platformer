using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public enum PlayerState { Idle, Run, Crouch, Jump, Fall, ClimbIdle, Climbing, Die }
public class PlayerController : MonoBehaviour
{
    [Header("FSM")]
    [SerializeField]
    private StateMachine<PlayerState> fsm;

    [SerializeField]
    private PlayerState curState;
    public PlayerState CurState { set { curState = value; } }

    private IdleState idleState;
    private RunState runState;
    private CrouchState crouchState;
    private JumpState jumpState;
    private FallState fallState;
    private ClimbIdleState climbIdleState;
    private ClimbingState climbingState;
    private DieState dieState;

    [Header("Components")]
    [SerializeField]
    private Animator animator;
    public Animator Animator { get { return animator; } }

    [SerializeField]
    private Rigidbody2D rigid;
    public Rigidbody2D Rigid { get { return rigid; } }

    [SerializeField]
    private SpriteRenderer spRenderer;
    public SpriteRenderer SpRenderer { get { return spRenderer; } }

    [Space(5)]

    // Move
    public const float MoveForce_Threshold = 0.1f;
    
    [Header("Propertys")]
    [SerializeField]
    private float movePower;
    public float MovePower { get { return movePower; } }

    [SerializeField]
    private float brakePower;
    public float BrakePower { get { return brakePower; } }

    [SerializeField]
    private float maxXVelocity;
    public float MaxXVelocity { get { return maxXVelocity; } }
    
    [SerializeField]
    private float maxYVelocity;
    public float MaxYVelocity { get { return maxYVelocity; } }

    private Vector2 moveDir;
    public Vector2 MoveDir { get { return moveDir; } }

    // Jump
    [SerializeField]
    private bool isJumped = false;
    [SerializeField]
    private float jumpSpeed;
    [SerializeField]
    private float powerJumpSpeed;
    [SerializeField]
    private bool isGround = true;
    [SerializeField]
    private bool powerJumpSet = false;
    [SerializeField]
    private int groundCount = 0;
    Coroutine powerJumpCo;
    [SerializeField]
    LayerMask groundCheckLayer;
    [SerializeField]
    LayerMask platformCheckLayer;
    [SerializeField]
    int platformLayer;
    [SerializeField]
    GameObject onPlatformObject;

    // Climb
    [SerializeField]
    private bool isClimbable = false;
    [SerializeField]
    private bool isClimb = false;
    [SerializeField]
    private float climbSpeed = 0;
    [SerializeField]
    LayerMask ladderCheckLayer;

    // Crouch
    [SerializeField]
    bool isCrouch = false;

    private void Awake()
    {
        fsm = new StateMachine<PlayerState>();
        fsm.AddState(PlayerState.Idle, idleState);
        fsm.AddState(PlayerState.Run, runState);
        fsm.AddState(PlayerState.Crouch, crouchState);
        fsm.AddState(PlayerState.Jump, jumpState);
        fsm.AddState(PlayerState.Fall, fallState);
        fsm.AddState(PlayerState.ClimbIdle, climbIdleState);
        fsm.AddState(PlayerState.Climbing, climbingState);
        fsm.AddState(PlayerState.Die, dieState);

        platformLayer = LayerMask.NameToLayer("Platform");
    }

    private void FixedUpdate()
    {
        switch(curState)
        {
            case PlayerState.Run:
                Move();
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        fsm.Update();

        switch(curState)
        {
            case PlayerState.Idle:
                break;
            case PlayerState.Run:
                
                break;
            case PlayerState.Crouch:
                break;
            case PlayerState.Jump:
                break;
            case PlayerState.Fall:
                break;
            case PlayerState.ClimbIdle:
                break;
            case PlayerState.Climbing:
                break;
            case PlayerState.Die:
                break;
        }
    }

    //private bool GetIsMoved(float value)
    //{
    //    return value > MoveForce_Threshold;
    //}

    private void Move()
    {
        // 입력을 받지 않는 경우
        if (moveDir.x == 0)
        {
            // 반발력 (브레이크 적용)
            if (rigid.velocity.x > MoveForce_Threshold)
                rigid.AddForce(Vector2.left * brakePower);
            else if (rigid.velocity.x < -MoveForce_Threshold)
                rigid.AddForce(Vector2.right * brakePower);
        }
        else
        {
            rigid.AddForce(Vector2.right * moveDir.x * movePower);

            // 최대 속력 제한 (수평)
            if (Mathf.Abs(rigid.velocity.x) > maxXVelocity)
                rigid.velocity = rigid.velocity.x < 0
                    ? new Vector2(-maxXVelocity, rigid.velocity.y) : new Vector2(maxXVelocity, rigid.velocity.y);

            // 최대 속력 제한 (수직)
            if (Mathf.Abs(rigid.velocity.y) > maxYVelocity)
                rigid.velocity = rigid.velocity.y < 0
                    ? new Vector2(rigid.velocity.y, -maxYVelocity) : new Vector2(rigid.velocity.x, maxYVelocity);
        }

        animator.SetFloat("YSpeed", rigid.velocity.y);
    }
    private void Jump(float value)
    {
        rigid.velocity = new Vector2(rigid.velocity.x, value);
    }
    private void JumpUnder()
    {
        StartCoroutine(IgnoreLayer());
    }
    private void Crouch(bool isCrouch)
    {
        this.isCrouch = isCrouch;
        animator.SetBool("IsCrouch", isCrouch);
    }
    private void Climb()
    {
        rigid.transform.Translate(new Vector2(0, moveDir.y * climbSpeed * Time.deltaTime));
    }
    private void SetClimb(bool isClimb)
    {
        rigid.gravityScale = isClimb ? 0 : 1;

        this.isClimb = isClimb;
        animator.SetBool("IsClimb", isClimb);
    }
    private void SetClimbable(bool isClimbable)
    {
        this.isClimbable = isClimbable;
    }
    private void SetIsGround(bool isGround)
    {
        groundCount = isGround ? groundCount + 1 : groundCount - 1;

        this.isGround = isGround;
        animator.SetBool("IsGround", isGround);
    }

    // Input System Call back
    private void OnMove(InputValue value)
    {
        if (isCrouch) return;
        if (isClimb) return;

        moveDir = value.Get<Vector2>();
        animator.SetFloat("MoveForce", Mathf.Abs(moveDir.x));

        if (moveDir.x < 0)
            spRenderer.flipX = true;
        else if (moveDir.x > 0)
            spRenderer.flipX = false;
    }
    private void OnJump(InputValue value)
    {
        if (!isGround) return;
        if (isClimb)
        {
            SetClimb(false);
            return;
        }
        if (isCrouch && !powerJumpSet && onPlatformObject != null)
        {
            Debug.Log("under jump");
            JumpUnder();
            Crouch(false);
            SetIsGround(false);
            return;
        }


        if (value.isPressed)
        {
            if (moveDir.x < MoveForce_Threshold && moveDir.x > -MoveForce_Threshold)
            {
                // 제자리 파워점프
                powerJumpSpeed = jumpSpeed;
                powerJumpCo = StartCoroutine(PowerJump());
                powerJumpSet = true;
            }
            else
            {
                // 이동중 점프
                powerJumpSpeed = 0;
                Jump(jumpSpeed);
            }
        }
        else
        {
            if (powerJumpCo != null)
                StopCoroutine(powerJumpCo);

            powerJumpSet = false;
            Jump(powerJumpSpeed);

            if (animator.GetBool("IsCrouch"))
                Crouch(false);
        }
    }
    private void OnCrouch(InputValue value)
    {
        if (!isGround) return;
        if (isClimb) return;
        if (rigid.velocity.magnitude != 0) return;

        // 토글형 앉기
        if (value.isPressed)
            Crouch(true);
        else
            Crouch(false);
    }
    private void OnClimb(InputValue value)
    {
        if (!isClimbable) return;
        rigid.velocity = Vector2.zero;

        moveDir.x = 0f;
        moveDir.y = value.Get<float>();
        Debug.Log(moveDir.y);
        animator.SetFloat("ClimbingSpeed", Mathf.Abs(moveDir.y));
        SetClimb(true);
        Climb();
    }

    // Collision Call back
    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
    private void OnCollisionExit2D(Collision2D collision)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (groundCheckLayer.Contain(collision.gameObject.layer))
            SetIsGround(true);
        if (ladderCheckLayer.Contain(collision.gameObject.layer))
            SetClimbable(true);
        if (platformCheckLayer.Contain(collision.gameObject.layer))
            onPlatformObject = collision.gameObject;

        Debug.Log("진입");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (groundCheckLayer.Contain(collision.gameObject.layer))
            SetIsGround(false);
        if (ladderCheckLayer.Contain(collision.gameObject.layer))
            SetClimbable(false);

        Debug.Log("탈출");
    }

    IEnumerator PowerJump()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            powerJumpSpeed += 0.05f;

            if (!animator.GetBool("IsCrouch"))
                Crouch(true);
        }
    }

    IEnumerator IgnoreLayer()
    {
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), onPlatformObject.GetComponent<TilemapCollider2D>(), true);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), onPlatformObject.GetComponent<TilemapCollider2D>(), false);
        onPlatformObject = null;
    }
}
