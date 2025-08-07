using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 0.4f; // 공격 범위 설정
    public int attackDamage = 5; // 공격력 설정

    public Transform RightattackPoint; // 우측 공격 위치 설정
    public Transform leftattackPoint; // 좌측 공격 위치 설정
    private Transform attackPoint; // 현재 공격 위치를 저장할 변수

    public LayerMask monsterLayer; // 공격 대상 레이어 설정
    public GameObject playerObject; // 플레이어 오브젝트 참조
    private PlayerMovement playerMovement; // 플레이어 이동 스크립트 참조
    private bool isFacingRight = true; // 플레이어가 바라보는 방향

    void Update()
    {
        // 플레이어 이동 스크립트에서 방향을 가져오기 위해 참조를 설정합니다.
        playerMovement = playerObject.GetComponent<PlayerMovement>();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // 플레이어가 공격 키(Q)를 눌렀을 때 공격 실행
            isFacingRight = playerMovement.IsFacingRight();
            if(isFacingRight ) {
                attackPoint = RightattackPoint; // 오른쪽 공격 위치 설정
            }
            else
            {
                attackPoint = leftattackPoint; // 왼쪽 공격 위치 설정
            }
            SwingSword();
        }
    }

    void SwingSword()
    {
        //attackPoint는 항상 오른쪽 기준으로 배치되어 있으니,
        // 왼쪽을 바라보면 Vector2.left 방향으로 계산된 offset만큼 위치 보정.
        // 좌우 방향에 따른 공격 위치 계산
        Vector3 attackPosition = attackPoint.position;
        if (transform.localScale.x < 0) // 왼쪽을 바라보는 경우
        {
            float mirroredX = transform.position.x - (attackPoint.position.x - transform.position.x);
            attackPosition = new Vector3(mirroredX, attackPoint.position.y, attackPoint.position.z);
        }
        // 원형 범위 체크
        Collider2D[] hitMonsters = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, monsterLayer);

        foreach (Collider2D monster in hitMonsters)
        {
            RedMonsterHealth red = monster.GetComponent<RedMonsterHealth>();
            if (red != null)
            {
                red.TakeDamage(attackDamage, transform.position);
                continue;
            }

            // YellowMonsterHealth
            YellowMonsterHealth yellow = monster.GetComponent<YellowMonsterHealth>();
            if (yellow != null)
            {
                yellow.TakeDamage(attackDamage, transform.position);
                continue;
            }

            // Red_High_MonsterHealth
            Red_High_MonsterHealth redHigh = monster.GetComponent<Red_High_MonsterHealth>();
            if (redHigh != null)
            {
                redHigh.TakeDamage(attackDamage, transform.position);
                continue;
            }

            // Yellow_High_MonsterHealth
            Yellow_High_MonsterHealth yellowHigh = monster.GetComponent<Yellow_High_MonsterHealth>();
            if (yellowHigh != null)
            {
                yellowHigh.TakeDamage(attackDamage, transform.position);
                continue;
            }

        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            // 좌우 방향 반영된 공격 위치로 Gizmo 그리기
            Vector3 gizmoPos = attackPoint.position;

            if (Application.isPlaying && transform.localScale.x < 0)
            {
                float mirroredX = transform.position.x - (attackPoint.position.x - transform.position.x);
                gizmoPos = new Vector3(mirroredX, attackPoint.position.y, attackPoint.position.z);
            }

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(gizmoPos, attackRange);
        }
    }
}

