using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton pattern to ensure only one instance of GameManager exists
    public static GameManager instance;
    // Total coin count
    public int coinCount = 0;
    public TextMeshProUGUI coinText;

    // Flag to check if the game is paused
    private bool isPaused = false;

    void Awake()
    {
        // Ensure that only one instance of GameManager exists
        if (instance == null)
            instance = this;
        else
            // If an instance already exists, destroy this one
            Destroy(gameObject);
        // Make sure the GameManager persists across scene loads
        DontDestroyOnLoad(gameObject);
    }

    public void PauseGame()
    {
        // 게임이 이미 일시정지 상태라면 아무 작업도 하지 않음
        if (isPaused) return;
        isPaused = true;

        // 플레이어 움직임 멈추기
        PlayerMovement playerMove = FindObjectOfType<PlayerMovement>();
        if (playerMove != null)
            playerMove.enabled = false;

        // 몬스터 움직임 멈추기
        MonsterPatrol[] monsters = FindObjectsOfType<MonsterPatrol>();
        foreach (var monster in monsters)
        {
            // 몬스터의 움직임을 멈추고 속도를 0으로 설정
            monster.enabled = false;
            Rigidbody2D rb = monster.GetComponent<Rigidbody2D>();
            // Rigidbody2D가 있다면 속도를 0으로 설정
            if (rb != null)
                rb.velocity = Vector2.zero;
        }

        // 게임 전반 타임스케일 멈추기 (필요시)
        // Time.timeScale = 0f;
    }

    public void AddCoin(int amount)
    {
        // Add coins to the total count and update the UI   
        coinCount += amount;
        UpdateCoinUI();
    }

    void UpdateCoinUI()
    {
        //  Update the coin text UI with the current coin count
        coinText.text = "Coins: " + coinCount+"/127개";
    }

    public void ResetCoins()
    {
        // Reset the coin count to zero and update the UI
        coinCount = 0;
        UpdateCoinUI();
    }

    public void RestartGame()
    {
        // 🔁 1. 게임 상태 초기화
        if (instance != null)
            instance.ResetCoins();  // 코인 초기화

        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
            playerHealth.ResetSurvivalTime();   // 생존 시간 초기화

        // 🔄 2. 씬 재시작
        Time.timeScale = 1f; // 혹시 멈춰 있었을 경우 복구
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}