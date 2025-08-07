using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePatrol : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float patrolDistance = 8f;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public float checkDistance = 0.2f;  // 적절한 거리로 조정

    private float flipCooldown = 0.2f;
    private float flipTimer = 0f;

    public Transform leftGroundCheck;   // 왼쪽 바닥 확인 위치
    public Transform rightGroundCheck;  // 오른쪽 바닥 확인 위치
    public Transform leftWallCheck;     // 왼쪽 벽 확인 위치
    public Transform rightWallCheck;    // 오른쪽 벽 확인 위치

    private Rigidbody2D rb;
    private Vector2 startPos;
    private bool movingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
    }

    void FixedUpdate()
    {
        flipTimer -= Time.fixedDeltaTime;
        Patrol();
    }

    void Patrol()
    {
        float moveDir = movingRight ? 1 : -1;
        rb.velocity = new Vector2(moveDir * moveSpeed, rb.velocity.y);

        RaycastHit2D wallHit = movingRight
            ? Physics2D.Raycast(rightWallCheck.position, Vector2.right, checkDistance, wallLayer)
            : Physics2D.Raycast(leftWallCheck.position, Vector2.left, checkDistance, wallLayer);

        RaycastHit2D groundHit = movingRight
            ? Physics2D.Raycast(rightGroundCheck.position, Vector2.down, checkDistance, groundLayer)
            : Physics2D.Raycast(leftGroundCheck.position, Vector2.down, checkDistance, groundLayer);

        bool shouldFlip = false;

        if (!groundHit.collider || wallHit.collider)
            shouldFlip = true;

        if (movingRight && transform.position.x >= startPos.x + patrolDistance - 0.01f)
            shouldFlip = true;
        else if (!movingRight && transform.position.x <= startPos.x - patrolDistance + 0.01f)
            shouldFlip = true;

        if (shouldFlip && flipTimer <= 0f)
        {
            FlipDirection();
            flipTimer = flipCooldown;
        }
    }

    void FlipDirection()
    {
        movingRight = !movingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void OnDrawGizmosSelected()
    {
        if (leftGroundCheck != null)
            Gizmos.DrawLine(leftGroundCheck.position, leftGroundCheck.position + Vector3.down * checkDistance);
        if (rightGroundCheck != null)
            Gizmos.DrawLine(rightGroundCheck.position, rightGroundCheck.position + Vector3.down * checkDistance);
        if (leftWallCheck != null)
            Gizmos.DrawLine(leftWallCheck.position, leftWallCheck.position + Vector3.left * checkDistance);
        if (rightWallCheck != null)
            Gizmos.DrawLine(rightWallCheck.position, rightWallCheck.position + Vector3.right * checkDistance);
    }
}
