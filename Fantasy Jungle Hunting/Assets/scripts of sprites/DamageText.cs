using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    public TextMeshProUGUI damageText;
    public float floatDuration = 0.5f;
    public float fadeDuration = 0.5f;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Initialize(int damage)
    {
        damageText.text = damage.ToString();
        StartCoroutine(AnimateText());
    }

    System.Collections.IEnumerator AnimateText()
    {
        Vector3 start = transform.position;
        Vector3 end = start + new Vector3(0, 0.5f, 0); // 위로 뜨는 효과
        float elapsed = 0f;

        // 떠오름
        while (elapsed < floatDuration)
        {
            transform.position = Vector3.Lerp(start, end, elapsed / floatDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 페이드 아웃
        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}

