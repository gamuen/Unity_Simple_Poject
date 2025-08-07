using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelUp : MonoBehaviour
{
    public GameObject player; // 플레이어 오브젝트
    public int currentLevel = 1;
    public int currentEXP = 0;
    public int expToNextLevel = 100; // 레벨업에 필요한 경험치
    public int maxLevel = 4; // 최대 레벨

    public int xpPerKill_1 = 10; // 적 1를 처치했을 때 얻는 경험치
    public int xpPerKill_2 = 16; // 적 2를 처치했을 때 얻는 경험치
    public int xpPerKill_3 = 27; // 적 3를 처치했을 때 얻는 경험치
    public int xpPerKill_4 = 65; // 적 4를 처치했을 때 얻는 경험치

    public int healthPerLevel = 50; // 레벨업 시 증가하는 체력
    public int attackPowerPerLevel = 2; // 레벨업 시 증가하는 공격력

    public Slider xpSlider;
    public TextMeshProUGUI levelText;


    // Start is called before the first frame update
    void Start()
    {
        PlayerHealth playerhealth = player.GetComponent<PlayerHealth>();
        PlayerAttack playerattack = player.GetComponent<PlayerAttack>();
        UpdateUI();
    }

    public void GainEXP(int amount)
    {
        if(currentLevel >= maxLevel) return;

        currentEXP += amount;
        PlayerHealth playerhealth = player.GetComponent<PlayerHealth>();
        PlayerAttack playerattack = player.GetComponent<PlayerAttack>();
        UpdateUI(); // UI 업데이트

        if (currentEXP >= expToNextLevel)
        {
            currentEXP -= 100;

            currentLevel++;
            playerhealth.maxHealth += healthPerLevel; // 레벨업 시 최대 체력이 증가하고,
            playerhealth.currentHealth = playerhealth.maxHealth; // 그 최대 체력으로 회복한다.
            playerhealth.UpdateHealthUI(); // UI 업데이트
            playerattack.attackDamage += attackPowerPerLevel; // 레벨업 시 공격력이 2만큼 증가한다.
            UpdateUI(); // UI 업데이트
        }

    }

    // Update is called once per frame
    void UpdateUI()
    {
        xpSlider.value = (float)currentEXP / expToNextLevel; // 경험치 슬라이더 업데이트
        levelText.text = "Level: " + currentLevel; // 레벨 텍스트 업데이트
    }
}
