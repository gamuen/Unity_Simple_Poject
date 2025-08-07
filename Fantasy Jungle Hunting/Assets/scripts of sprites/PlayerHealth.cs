using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    // maxHealth 는 플레이어가 취할 수 있는 최대 체력 값을 의미하고, currenthealth 는 이름처럼 현재의 체력 값을 의미하는 실수입니다. 슬라이더 속성은 currenthealth 값을 표시하고요.
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthSlider;

    // damageTextPrefab 는 데미지 텍스트 프리팹을 의미하고, textSpawnPoint 는 데미지 텍스트가 생성될 위치를 의미합니다.
    public GameObject damageTextPrefab; // 데미지 텍스트 프리팹
    public Transform textSpawnPoint;    // 머리 위 위치

    // 게임 오버 UI 패널과 코인 수, 생존 시간을 표시할 텍스트 컴포넌트입니다.
    public GameObject gameOverPanel;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI survivalTimeText;

    private float survivalTime;
    private bool isDead = false;
    // start 함수에서는 currenthealth 는 max 상태로 초기화하고, UpdateHealthUI 함수를 실행해서
    // 현재 페력이 최대 체력의 몇 퍼센트인지를 슬라이더로써 업데이트하며 표시하게 합니다.
    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        // 게임 오버 패널 비활성화함,
        gameOverPanel.SetActive(false);
        // 코인 수와 생존 시간 초기화
        survivalTime = 0f;
    }
    // TakeDamage 함수는 받은 데미지인 amount 의 값만큼을 현재 체력 값에서 차감하고,
    // knockBackDir 의 방향벡터의 방향을 갖고 knockBackForce 크기만큼 x 축 방향으로
    // AddForce 의 힘을 가하게 만드는 함수입니다. 

    void Update()
    {
        if (!isDead)
        {// 플레이어가 죽지 않은 상태일 때에만 생존 시간을 업데이트합니다.
            survivalTime += Time.deltaTime;
        }
    }
    public void TakeDamage(int amount, Vector2 knockbackDir, float knockbackForce)
    {
        if (isDead) return;
        currentHealth -= amount;
        // 단, 체력이떨어지되 0 미만으로 현재 체력 값이 떨어지지 않게 합니다. 
        currentHealth = Mathf.Max(currentHealth, 0);

        // 데미지 텍스트 생성
        ShowDamageText(amount);

        // 넉백 처리는 rb 가 null 이 아닐 때에만 가능합니다. 
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
        }
        // 차감된 현재 체력은 체력 slider 업데이트를 통해 즉시 유저에게 보여줍니다. 
        UpdateHealthUI();

        //  만약 현재 체력이 0이 되면 Die 함수를 호출하여 플레이어가 죽은 상태로 전환합니다.
        if (currentHealth == 0)
        {
            Die();
        }
    }

    void ShowDamageText(int amount)
    {
        // 데미지 텍스트 프리팹과 텍스트 스폰 포인트가 설정되어 있을 때에만 데미지 텍스트를 생성합니다.
        if (damageTextPrefab != null && textSpawnPoint != null)
        {
            GameObject dmg = Instantiate(damageTextPrefab, textSpawnPoint.position, Quaternion.identity);
            dmg.GetComponent<DamageText>().Initialize(amount);
        }
    }
    // UpdateHealthUI 함수는 healthSlider 값이 존재할 때, 해당 값을 currenthealth 값을 최대 페력 값으로 나눈 1 이하의 실수 값으로 업데이트합니다.
    public void UpdateHealthUI()
    {
        // 슬라이더가 null이 아닐 때에만 업데이트합니다.
        if (healthSlider != null)
        {
            // 슬라이더의 값은 현재 체력을 최대 체력으로 나눈 값으로 설정합니다.
            healthSlider.value = (float)currentHealth / maxHealth;
        }
    }

    public void ResetSurvivalTime()
    {
        // 생존 시간을 초기화합니다.
        survivalTime = 0f;
        isDead = false;
    }

    void Die()
    {
        isDead = true;

        // 게임 오버 UI 활성화
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // 코인 수 표시 (GameManager에서 코인 수 가져오기)
        int totalCoins = GameManager.instance != null ? GameManager.instance.coinCount : 0;
        if (coinText != null)
            coinText.text = "획득한 코인: " + totalCoins;

        // 생존 시간 표시 (초 단위, 소수점 둘째 자리까지)
        if (survivalTimeText != null)
            survivalTimeText.text = "생존 시간: " + survivalTime.ToString("F2") + "초";

        // 움직임 정지 호출입니다.
        GameManager.instance.PauseGame();
    }

    public void Heal(int amount)
    // Heal 함수는 amount 만큼 현재 체력을 회복시키는 함수입니다.
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }
}
