using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaidInOut : MonoBehaviour
{
    public Image img;
    float time = 0f;
    float F_time = 1f;

    public void Fade()
    {
        StartCoroutine(FadeFlow());
    }
    IEnumerator FadeFlow()
    {
        img.gameObject.SetActive(true);
        time = 0f;
        Color alpha = img.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            img.color = alpha;
            yield return null;
        }
        time = 0f;

        yield return new WaitForSeconds(1f);

        while (alpha.a > 0f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            img.color = alpha;
            yield return null;
        }
        img.gameObject.SetActive(false);

        yield return null;

    }
}
