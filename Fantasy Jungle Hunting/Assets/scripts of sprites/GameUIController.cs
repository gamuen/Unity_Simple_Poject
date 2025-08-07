using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIController : MonoBehaviour
{
    public Button restartButton;
    public TextMeshProUGUI restartButtonText;

    void Start()
    {

        // 텍스트 변경
        if (restartButtonText != null)
        {
            restartButtonText.text = "게임재시작";
        }
        // 버튼 클릭 시 GameManager.Instance.RestartGame() 실행
        restartButton.onClick.AddListener(() =>
        {
            if (GameManager.instance != null)
                GameManager.instance.RestartGame();
        });
    }
}
