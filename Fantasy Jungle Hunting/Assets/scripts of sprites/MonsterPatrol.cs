using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MonsterPatrol : MonoBehaviour
{
    // 몬스터의 이동 속도, 순찰 거리, 바닥 및 벽 레이어 설정
    public float moveSpeed = 2f;
    public float patrolDistance = 8f;
    // 바닥과 벽을 확인하기 위한 레이어 마스크
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    // 몬스터가 벽과 바닥을 확인하기 위한 거리
    public float checkDistance = 0.5f;

    // 몬스터가 방향을 전환할 때의 쿨다운 시간 설정
    private float flipCooldown = 0.2f;
    // 몬스터가 방향을 전환할 때까지의 타이머
    private float flipTimer = 0f;

    public Transform leftWallCheck;     // 왼쪽 벽 확인 위치
    public Transform rightWallCheck;    // 오른쪽 벽 확인 위치
    public Transform leftGroundCheck;   // 왼쪽 바닥 확인 위치
    public Transform rightGroundCheck;  // 오른쪽 바닥 확인 위치

    private Rigidbody2D rb;
    // 몬스터가 시작한 위치
    private Vector2 startPos;
    // 몬스터가 현재 이동 방향 (오른쪽: true, 왼쪽: false)
    private bool movingRight = true;

    void Start()
    {
        // Rigidbody2D 컴포넌트 가져오기
        rb = GetComponent<Rigidbody2D>();
        // 몬스터의 시작 위치를 현재 위치로 설정
        startPos = transform.position;
    }

    void FixedUpdate()
    {
        // 타이머가 0보다 크면 감소시키고, 그렇지 않으면 순찰을 계속함
        flipTimer -= Time.fixedDeltaTime;
        Patrol();
    }

    void Patrol()
    {
        // 몬스터가 이동 방향에 따라 속도를 설정하고, 벽과 바닥을 확인함.
        float moveDir = movingRight ? 1 : -1;
        rb.velocity = new Vector2(moveDir * moveSpeed, rb.velocity.y);

        // 벽과 바닥을 확인하기 위한 레이캐스트이다.
        RaycastHit2D wallHit = movingRight
            ? Physics2D.Raycast(rightWallCheck.position, Vector2.right, checkDistance, wallLayer)
            : Physics2D.Raycast(leftWallCheck.position, Vector2.left, checkDistance, wallLayer);

        // 바닥을 확인하기 위한 레이캐스트이다.
        RaycastHit2D groundHit = movingRight
            ? Physics2D.Raycast(rightGroundCheck.position, Vector2.down, checkDistance, groundLayer)
            : Physics2D.Raycast(leftGroundCheck.position, Vector2.down, checkDistance, groundLayer);

        // 몬스터가 벽에 부딪히거나 바닥이 없는 경우 방향을 전환해야 하는지 확인함.
        bool shouldFlip = false;

        // 벽에 부딪히거나 바닥이 없는 경우 방향을 전환함.
        if (!groundHit.collider || wallHit.collider)
            shouldFlip = true;

        // 몬스터가 순찰 거리를 벗어났는지 확인함.
        if (movingRight && transform.position.x >= startPos.x + patrolDistance)
            shouldFlip = true;
        //  몬스터가 왼쪽으로 이동 중이고 순찰 거리를 벗어났는지 확인함.
        else if (!movingRight && transform.position.x <= startPos.x - patrolDistance)
            shouldFlip = true;

        // 방향을 전환해야 하는 경우, 쿨다운 타이머가 0보다 작거나 같으면 방향을 전환함.
        if (shouldFlip && flipTimer <= 0f)
        {
            // 방향 전환 함수 호출함.
            FlipDirection();
            // 쿨다운 타이머를 초기화함.
            flipTimer = flipCooldown;
        }
    }

    void FlipDirection()
    {
        // 방향 전환: 이동 방향을 반대로 바꾸고 스케일을 뒤집는다.
        movingRight = !movingRight;
        // 스케일을 뒤집어 몬스터가 반대 방향을 바라보도록 한다.
        Vector3 scale = transform.localScale;
        // x축 스케일을 반전시켜 몬스터가 반대 방향을 바라보도록 한다.
        scale.x *= -1;
        // 몬스터의 스케일을 업데이트한다.
        transform.localScale = scale;
    }

    void OnDrawGizmosSelected()
    {
        // Gizmos를 사용하여 몬스터의 순찰 범위와 벽 및 바닥 확인 위치를 시각적으로 표시함
        if (leftGroundCheck != null)
            Gizmos.DrawLine(leftGroundCheck.position, leftGroundCheck.position + Vector3.down * checkDistance);
        // 오른쪽 바닥 확인 위치를 시각적으로 표시함
        if (rightGroundCheck != null)
            Gizmos.DrawLine(rightGroundCheck.position, rightGroundCheck.position + Vector3.down * checkDistance);
        // 왼쪽 벽 확인 위치를 시각적으로 표시함
        if (leftWallCheck != null)
            Gizmos.DrawLine(leftWallCheck.position, leftWallCheck.position + Vector3.left * checkDistance);
        // 오른쪽 벽 확인 위치를 시각적으로 표시함
        if (rightWallCheck != null)
            Gizmos.DrawLine(rightWallCheck.position, rightWallCheck.position + Vector3.right * checkDistance);
    }
}
