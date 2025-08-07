using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public AudioClip coinSound; // Inspector에서 할당할 AudioClip
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 효과음 재생
            audioSource.PlayOneShot(coinSound);

            // 코인 수 +1
            GameManager.instance.AddCoin(1);

            // 오브젝트를 곧바로 제거하면 소리가 안 들리므로
            // 비활성화 후 파괴 (소리 재생 시간 후 파괴)
            GetComponent<SpriteRenderer>().enabled = false;  // 코인 시각적으로 제거
            GetComponent<Collider2D>().enabled = false;      // 더 이상 충돌 안 되게

            Destroy(gameObject, coinSound.length);           // 소리 길이만큼 기다린 후 제거
        }
    }
}
