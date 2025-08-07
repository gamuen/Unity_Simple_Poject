using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButtonHandler : MonoBehaviour
{
    public void OnRestartButtonClicked()
    {
        // 코인 수 초기화
        if (GameManager.instance != null)
            GameManager.instance.ResetCoins();

        // 생존 시간 초기화
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
            playerHealth.ResetSurvivalTime();

        // 씬 재로딩
        Time.timeScale = 1f; // 혹시 정지되어 있다면 복구
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
