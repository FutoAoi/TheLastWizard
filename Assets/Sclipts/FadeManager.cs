using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup;
    public float fadeTime = 1.0f;

    void Start()
    {
        StartCoroutine(FadeIn());
    }
    IEnumerator FadeIn()
    {
        float time = 0f;
        while (time < fadeTime)
        {
            fadeCanvasGroup.alpha = 1 - (time / fadeTime);
            time += Time.deltaTime;
            yield return null;
        }
        fadeCanvasGroup.alpha = 0;
    }

    public void FadeOutAndLoadScene()
    {
        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeOut()
    {
        float time = 0f;

        while (time < fadeTime)
        {
            fadeCanvasGroup.alpha = time / fadeTime;
            time += Time.deltaTime;
            yield return null;
        }

        fadeCanvasGroup.alpha = 1;

        SceneManager.LoadScene(1);
    }
}
