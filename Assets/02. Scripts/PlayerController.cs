using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    Animator animator;
    [SerializeField]
    Rigidbody2D rigid;
    [SerializeField]
    SpriteRenderer spRenderer;
    [SerializeField]
    Sprite sprClimb;

    [Space(5)]

    // Move
    const float MoveForce_Threshold = 0.1f;
    [Header("Propertys")]
    [SerializeField]
    float movePower;
    [SerializeField]
    float brakePower;
    [SerializeField]
    float maxXVelocity;
    [SerializeField]
    float maxYVelocity;

    private Vector2 moveDir;

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
        platformLayer = LayerMask.NameToLayer("Platform");
    }

    private void FixedUpdate()
    {
        if (isClimb)
            Climb();
        else
            Move();
    }

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
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), onPlatformObject.GetComponent<Collider2D>(), true);
        StartCoroutine(IgnoreLayer(onPlatformObject));
    }
    private void Crouch(bool isCrouch)
    {
        this.isCrouch = isCrouch;
        animator.SetBool("IsCrouch", isCrouch);
    }
    private void Climb()
    {
        animator.SetFloat("ClimbingSpeed", Mathf.Abs(moveDir.y));

        // 입력을 받지 않는 경우
        if (moveDir.y == 0)
        {
            // 반발력 (브레이크 적용)
            if (rigid.velocity.y > MoveForce_Threshold)
                rigid.AddForce(Vector2.down * brakePower);
            else if (rigid.velocity.y < -MoveForce_Threshold)
                rigid.AddForce(Vector2.up * brakePower);
        }
        else
        {
            rigid.AddForce(Vector2.up * moveDir.y * climbSpeed);

            // 최대 속력 제한 (수직)
            if (Mathf.Abs(rigid.velocity.y) > maxYVelocity)
                rigid.velocity = rigid.velocity.y < 0
                    ? new Vector2(rigid.velocity.y, -maxYVelocity) : new Vector2(rigid.velocity.x, maxYVelocity);
        }
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
        SetClimb(true);
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

    IEnumerator IgnoreLayer(GameObject gameObject)
    {
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), onPlatformObject.GetComponent<Collider2D>(), false);
        onPlatformObject = null;
    }
}
