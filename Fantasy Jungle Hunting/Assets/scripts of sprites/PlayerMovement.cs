using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;       // 좌우 이동 속도
    public float jumpForce = 7f;       // 점프 힘
    public LayerMask groundLayer; // 바닥 감지용 레이어
    private bool isFacingRight = true;

    private Rigidbody2D rb;
    private bool isGrounded;

    public Transform groundCheck;      // 바닥 체크용 위치
    public float groundCheckRadius = 0.2f;

    private Animator animator;         // 애니메이터
    private SpriteRenderer spriteRenderer; // 방향 반전을 위한 스프라이트 렌더러
    public SpriteRenderer headSpriteRenderer; // 방향 반전을 위한 머리 스프라이트 렌더러.
    public SpriteRenderer swordSpriteRenderer; // 검 스프라이트 렌더러.

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 좌우 이동
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // ✅ Speed 파라미터 설정 (절댓값)
        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        // ✅ 좌우 방향 전환 (flipX)
        if (moveInput < 0)
        {
            spriteRenderer.flipX = true;  // 왼쪽 방향으로 몸통 스프라이트 렌더러 반전.
            headSpriteRenderer.flipX = true; // 머리 스프라이트도 왼쪽으로 반전.
            swordSpriteRenderer.flipX = true; // 검 스프라이트도 왼쪽으로 반전.
            isFacingRight = false; // 왼쪽을 바라보고 있음을 표시.
        }
        else if (moveInput > 0)
        {
            spriteRenderer.flipX = false; // 오른쪽 몸통 반전.
            headSpriteRenderer.flipX = false; // 머리 스프라이트도 오른쪽으로 반전.
            swordSpriteRenderer.flipX = false; // 검 스프라이트도 오른쪽으로 반전.
            isFacingRight = true; // 오른쪽을 바라보고 있음을 표시.
        }

        // 바닥에 있는지 확인
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Spacebar 눌렀을 때 점프
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

    }
    public bool IsFacingRight()
    {
        return isFacingRight;
    }
}
