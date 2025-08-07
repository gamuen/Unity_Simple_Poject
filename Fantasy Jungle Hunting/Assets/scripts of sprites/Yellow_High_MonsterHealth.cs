using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Yellow_High_MonsterHealth : MonoBehaviour
{
    private bool isDead = false;
    // 몬스터의 최대 체력과 현재 체력을 저장하는 변수
    public int maxHealth = 135;
    public int currentHealth;
    // UI 슬라이더를 통해 체력을 표시하기 위한 변수
    public Slider healthBar;
    private Rigidbody2D rb;
    // 몬스터의 넉백 상태를 나타내는 변수
    private bool isKnockedBack = false;

    // 몬스터가 죽었을 때 생성할 코인 프리팹과 사운드 효과를 저장하는 변수
    public GameObject coinPrefab;
    public AudioClip deathSFX;
    private AudioSource audioSource;
    // 몬스터의 스프라이트 렌더러를 저장하는 변수
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // 몬스터의 최대 체력을 설정하고 현재 체력을 최대 체력으로 초기화
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        // Rigidbody2D 컴포넌트를 가져와서 몬스터의 물리적 동작을 처리합니다.
        audioSource = GetComponent<AudioSource>();
        // 스프라이트 렌더러를 가져와 몬스터의 외형을 관리합니다.
        spriteRenderer = GetComponent<SpriteRenderer>();
        // UI 슬라이더를 찾아서 초기화합니다.
        UpdateHealthUI();
    }

    public void TakeDamage(int amount, Vector2 attackerPosition)
    {
        if (isDead) return;
        // 데미지가 발생 시, 몬스터의 체력을 감소시킴
        currentHealth -= amount;
        // 체력 0 이상으로 제한한다.
        currentHealth = Mathf.Max(0, currentHealth);
        // 체력이 0 이하가 되면 몬스터를 제거
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        // 넉백
        if (!isKnockedBack)
        {
            // 넉백 방향 계산: 공격자 위치에서 몬스터 위치로의 단위 방향 벡터를 3차원 벡터.normalized 를 통해 구합니다.
            Vector2 knockbackDir = (transform.position - (Vector3)attackerPosition).normalized;
            StartCoroutine(KnockbackCoroutine(knockbackDir));
        }
    }

    void UpdateHealthUI()
    {
        // UI 슬라이더 업데이트
        if (healthBar != null)
            healthBar.value = (float)currentHealth / maxHealth;
    }
    // 넉백 코루틴으로, 몬스터가 넉백되는 동안의 동작을 처리합니다.
    System.Collections.IEnumerator KnockbackCoroutine(Vector2 direction)
    {
        isKnockedBack = true;
        Vector2 originalPos = rb.position;
        // 넉백 방향으로 3픽셀 이동한 위치를 타겟 위치로 선정.
        Vector2 targetPos = originalPos + direction * 0.03f; // 3픽셀 = 0.03 유닛
        // 넉백이 시작되는 시간 설정
        float elapsed = 0f;
        // 이동하는 총 시간을 0.5초로 설정합니다.
        float duration = 0.5f;

        while (elapsed < duration)
        {
            // 이동 중에 Rigidbody2D를 사용하여 위치를 타겟 위치 값을 향해 업데이트합니다.
            rb.MovePosition(Vector2.Lerp(originalPos, targetPos, elapsed / duration));
            // 시간 경과를 계산합니다.
            elapsed += Time.deltaTime;
            // 각 프레임마다 잠시 대기합니다.
            yield return null;
        }
        // 몬스터가 넉백에 의해 타겟 위치로 이동하는 코드입니다.
        rb.MovePosition(targetPos);
        isKnockedBack = false;
    }

    void Die()
    {
        if (isDead) return; // ✅ Die()가 중복 실행되지 않도록
        isDead = true;      // ✅ 한 번만 실행되도록 설정
        // ▶ 플레이어 체력 회복을 위해 player 태그가 붙은 게임 오브젝트를 찾습니다.
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            // 플레이어의 체력을 5만큼 회복합니다.
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(11);
            }

            PlayerLevelUp exp = player.GetComponent<PlayerLevelUp>();
            if (exp != null)
            {
                // 플레이어 레벨업 컴포넌트가 있다면, 경험치를 20만큼 증가시킵니다.
                exp.GainEXP(exp.xpPerKill_3);
            }
        }

        // ▶ 사망 효과음을 Die 함수 실행이 될 때, 재생하며, 이 함수는 currenthealth 가 0 이하가 되었을 때에만 실행됩니다.
        if (deathSFX != null && audioSource != null)
            audioSource.PlayOneShot(deathSFX);

        // ▶ 코인 프리팹을 자신의 위치에 생성합니다.
        if (coinPrefab != null)
            Instantiate(coinPrefab, transform.position, Quaternion.identity);

        // ▶ 몬스터 오브젝트를 페이드 아웃 후 제거하는 함수도 실행합니다.
        StartCoroutine(FadeOutAndDestroy());
    }

    IEnumerator FadeOutAndDestroy()
    {
        // 몬스터의 스프라이트 렌더러를 가져옵니다.
        float duration = 0.5f;

        float timer = 0f;

        while (timer < duration)
        {
            // 스프라이트의 알파 값을 점점 줄여서 페이드 아웃 효과를 줍니다.
            float alpha = Mathf.Lerp(1f, 0f, timer / duration);
            if (spriteRenderer != null)

                // 스프라이트 렌더러가 null 이 아닐 때에만 색상을 변경합니다.
                spriteRenderer.color = new Color(1f, 1f, 1f, alpha);
            // 페이드 아웃 효과를 주기 위해 알파 값을 점점 줄입니다.
            timer += Time.deltaTime;
            yield return null;
            // 페이드 아웃이 완료되면, 스프라이트의 알파 값을 0으로 설정합니다.
        }

        Destroy(gameObject);
        // 몬스터 오브젝트를 제거합니다.
    }
}

