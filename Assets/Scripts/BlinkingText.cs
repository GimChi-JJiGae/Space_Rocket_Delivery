using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingText : MonoBehaviour
{
    public Text textComponent;
    public Color[] colors;
    public float duration = 1.0f;

    private void Start()
    {
        if (textComponent == null)
        {
            textComponent = GetComponent<Text>();
        }

        InitializeBlinking(); // 메서드 호출 추가
    }

    // InitializeBlinking 메서드 추가
    public void InitializeBlinking()
    {
        StopAllCoroutines();
        StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        int currentColorIndex = 0;

        while (true)
        {
            float lerp = Mathf.PingPong(Time.unscaledTime / duration, 1.0f); // 변경된 부분
            Color startColor = colors[currentColorIndex];
            Color endColor = colors[(currentColorIndex + 1) % colors.Length];

            // 알파값을 1로 초기화
            startColor.a = 1.0f;
            endColor.a = 1.0f;

            textComponent.color = Color.Lerp(startColor, endColor, lerp);

            if (lerp >= 0.999f)
            {
                currentColorIndex = (currentColorIndex + 1) % colors.Length;
            }

            yield return null;
        }
    }

}
