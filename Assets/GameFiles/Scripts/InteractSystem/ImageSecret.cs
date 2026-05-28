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
        
        SetAlpha(endAlpha);
    }

    void SetAlpha(float alpha)
    {
        if (image != null)
        {
            Color colorImage = image.color;
            colorImage.a = alpha;
            image.color = colorImage;
        }
        
        if (text1 != null)
        {
            Color colorText1 = text1.color;
            colorText1.a = alpha;
            text1.color = colorText1;
        }
        
        if (text2 != null)
        {
            Color colorText2 = text2.color;
            colorText2.a = alpha;
            text2.color = colorText2;
        }
    }
}