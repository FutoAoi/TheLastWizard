using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleUIManager : MonoBehaviour
{
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _endButton;
    [SerializeField] private FadeManager _fadeManager;
    private void Start()
    {
        _newGameButton.onClick.AddListener(NewGame);
        _endButton.onClick.AddListener(EndGame);
        AudioManager.Instance.PlayBGM("Title");
    }

    private void NewGame()
    {
        _fadeManager.FadeOutAndLoadScene();
    }


    private void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}
