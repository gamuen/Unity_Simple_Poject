using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMonsterAttack : MonoBehaviour
{
    // attackRange 는 몬스터가 공격가능한 사거리의 최대 값을 뜻하는 양의 실수 값이고, 
    // damage 는 몬스터가 플레이어에게 1회 피격 시에 가하는 데미지의 총량을 뜻합니다.
    // attackCooldown 은 공격하는 쿨다운 주기 시간 값을 뜻하는 양의 실수이고,
    // knockbackForce 는 플레이어에게 데미지를 가할 때 플레이어에게 주는 넉백 힘의 크기로,
    // x 축 방향의 힘 벡터 크기를 뜻합니다. 
    public float attackRange = 1.5f;
    public int damage = 7;
    public float attackCooldown = 1.5f;
    public float knockbackForce = 5f;
    // lastAttackTime 은 마지막으로 플레이어를 공격했던 시각 값으로, play 직후를 기점으로
    // 초 단위로 기록되는 시각 값입니다. 
    private float lastAttackTime;
    // player 가 있는 현재 위치를 상대적 위치로써 기록하는 transform 속성을 하나 만듭니다.
    public Transform player;
    // Update 함수에서는 player 가 존재할 경우 이 몬스터 오브젝트의 transform.position 값과
    // 플레이어의 position 값을 2d 내의 놈 벡터의 크기 값으로써 계산하는 유클리드 거리 계산 함수인, Vector2 템플릿의 Distance 함수를 사용해 계산합니다.
    void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);
        // 이 거리가 attackRange 보다 작은 값일 경우 AttackPlayer 함수를 실행시켜 player 에게 데미지를 주고 넉백 힘 벡터를 가합니다. 단, 이때 자신이 마지막으로 공격한 시각 값과
        // 현재 시각을 비교한 값이 공격 주기보다 클 때에만 이 작업을 실행시켜 쿨다운 주기를 부여합니다. 
        if (dist <= attackRange && Time.time - lastAttackTime > attackCooldown)
        {
            AttackPlayer();
            // 이어서 마지막으로 공격한 시각 값을 현재 시각으로 갱신합니다. 
            lastAttackTime = Time.time;
        }
    }

    void AttackPlayer()
    {
        // AttackPlayer 함수는 ph 라는 playerHealth 객체를 생성하고 Playerhealth 속성을 
        // 외부 컴포넌트인 PlayerHealth.cs 파일에서 가져옵니다. 
        PlayerHealth ph = player.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            // ph 를 가져오는데 성공하면, knockbackDir: 즉 넉백 방향 벡터의 방향 값을 
            // 상대적 위치를 통해 계산하고 Vector2 의 normalized 속성을 통해 절댓값의 크기가
            // 1 이하가 되게끔 계산합니다. 
            Vector2 knockbackDir = (player.position - transform.position).normalized;
            // 이어서 Playerhealth 객체의 TakeDamage 함수를 실행시켜 1.5 크기만큼의 backForce 와 
            // 계산한 방향으로의 넉백 힘 벡터를 가하면서, damage=10 만큼의 체력을 현재 체력에서
            // 차감시키는 함수입니다. 
            ph.TakeDamage(damage, knockbackDir, knockbackForce);
        }
    }
}
