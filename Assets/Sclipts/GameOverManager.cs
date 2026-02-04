using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    public CanvasGroup gameOverPanel;

    public CanvasGroup gameClearPanel;

    public CanvasGroup fadePanel;

    public float fadeTime = 1.0f;

    public float displayTime = 2.0f;

    public void StartGameOver()
    {
        StartCoroutine(ResultSequence(gameOverPanel));
    }

    public void StartGameClear()
    {
        StartCoroutine(ResultSequence(gameClearPanel));
    }

    IEnumerator ResultSequence(CanvasGroup resultPanel)
    {
        float time = 0f;

        while (time < fadeTime)
        {
            resultPanel.alpha = time / fadeTime;
            time += Time.deltaTime;
            yield return null;
        }
        resultPanel.alpha = 1;

        yield return new WaitForSeconds(displayTime);

        time = 0f;
        while (time < fadeTime)
        {
            fadePanel.alpha = time / fadeTime;
            time += Time.deltaTime;
            yield return null;
        }
        fadePanel.alpha = 1;

        GameManager.instance.Mause(false);
        SceneManager.LoadScene(0);
    }
}
