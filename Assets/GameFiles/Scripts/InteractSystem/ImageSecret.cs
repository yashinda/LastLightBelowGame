using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIFadeController : MonoBehaviour
{
    public Image image;
    public TMP_Text text1;
    public TMP_Text text2;

    private void Start()
    {
        StartCoroutine(FadeSequence());
    }

    IEnumerator FadeSequence()
    {
        yield return StartCoroutine(Fade(0f, 1f, 1f));

        yield return new WaitForSeconds(3f);

        yield return StartCoroutine(Fade(1f, 0f, 1f));
        gameObject.SetActive(false);
    }

    IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, time / duration);
            SetAlpha(alpha);

            time += Time.deltaTime;
            yield return null;
        }

        // Гарантируем точное конечное значение
        SetAlpha(endAlpha);
    }

    void SetAlpha(float alpha)
    {
        // Image
        if (image != null)
        {
            Color c = image.color;
            c.a = alpha;
            image.color = c;
        }

        // Text1
        if (text1 != null)
        {
            Color c = text1.color;
            c.a = alpha;
            text1.color = c;
        }

        // Text2
        if (text2 != null)
        {
            Color c = text2.color;
            c.a = alpha;
            text2.color = c;
        }
    }
}