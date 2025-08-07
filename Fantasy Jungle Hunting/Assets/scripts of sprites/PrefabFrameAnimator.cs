using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabFrameAnimator : MonoBehaviour
{
    public GameObject[] frames;                  // 프레임 프리팹들 (하나의 배열만 사용)
    public float frameDuration = 0.03f;          // 프레임 간 시간
    public GameObject playerObject;              // 실제 플레이어 오브젝트 (애니메이션 전용 아님)

    private int currentFrame = 0;
    private float timer = 0f;
    private bool isPlaying = false;
    private PlayerMovement playerMovement;
    private bool isFacingRight = true;

    void Start()
    {
        playerMovement = playerObject.GetComponent<PlayerMovement>();
        HideAllFrames();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q) && !isPlaying)
        {
            isFacingRight = playerMovement.IsFacingRight();
            // 애니메이션 시작했다는 표시하기.
            isPlaying = true;
            // 현재 프레임의 인덱스를 0으로 초기화하고 타이머를 0 초로 초기화합니다.
            currentFrame = 0;
            timer = 0f;
            // 모든 플레이어의 상위 sprite 컴포넌트 자체는 숨깁니다.
            playerObject.GetComponent<SpriteRenderer>().enabled = false;
        
            ShowFrame(currentFrame);
        }

        if (isPlaying)
        {
            timer += Time.deltaTime;
            // 현재 프레임의 시간과 프레임 간 설정 시간 0.3초를 비교함.
            if (timer >= frameDuration)
            {
                // 0.3초로 세팅해 둔 프레임 시간이 초과될 시 다음 프레임으로 이동하면서 timer 를 리셋함.
                timer = 0f;
                currentFrame++;

                if (currentFrame >= frames.Length)
                {
                    // 애니메이션이 끝났을 때
                    isPlaying = false;
                    // 현재 프레임을 0으로 리셋
                    HideAllFrames();
                    // 플레이어 오브젝트의 스프라이트 렌더러를 다시 활성화
                    playerObject.GetComponent<SpriteRenderer>().enabled = true;
                    return;
                }

                ShowFrame(currentFrame);
            }
        }
    }

    void ShowFrame(int index)
    {
        // ShowFrame 함수는 현재 프레임을 활성화하고 나머지 프레임은 모두 SetActive 고유 라이브러리 함수를 통해 비활성화합니다.
        for (int i = 0; i < frames.Length; i++)
        {
            // 현재 프레임의 인덱스와 일치하는 경우에만 해당 프레임을 활성화합니다.
            bool isCurrent = (i == index);
            // 현재 프레임을 활성화하고 나머지 프레임은 비활성화합니다.
            frames[i].SetActive(isCurrent);

            if (isCurrent)
            {
                // 방향에 따라 flipX 값을 설정합니다.
                // 🔁 프레임 내부의 모든 SpriteRenderer에 대해 flipX 설정
                SpriteRenderer[] renderers = frames[i].GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer renderer in renderers)
                {
                    renderer.flipX = !isFacingRight;
                }
            }

        }
    }

    void HideAllFrames()
    {
        // HideAllFrames 함수는 모든 프레임을 비활성화합니다.
        foreach (GameObject frame in frames)
        {
            frame.SetActive(false);
        }
    }
}
