using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private List<ICharactor> _characterList = new List<ICharactor>();
    [SerializeField] private GamePhase _currentGamePhase = GamePhase.Break;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private TMP_Text _pheseText;
    [SerializeField] private IconDataBase _iconData;
    [SerializeField] private float _phaseChangeTime;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private Material _nightMaterial;
    [SerializeField] private Material _morningMaterial;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private TMP_Text _midText;
    [SerializeField] private GameOverManager _gameOverManager;
    private float _timer = 0;
    public event Action PhaseChange_Break;
    public event Action PhaseChange_Battle;
    private PlayerController _playerController;

    public IconDataBase IconData => _iconData;
    public UIManager UIManager => _uiManager;
    public GamePhase CurrentGamePhase => _currentGamePhase;

    [SerializeField, Tooltip("フェード用パネル")]
    private Image _fadeImage;

    [SerializeField, Tooltip("フェード時間")]
    private float _fadeDuration = 1f;

    private Coroutine _fadeCoroutine;

    void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        RenderSettings.skybox = _morningMaterial;
        AudioManager.Instance.PlayBGM("Hiru");
        _playerController = FindAnyObjectByType<PlayerController>();
        _midText.text = "目標 : " + "コアに触れて次の襲撃の準備をする";
        Mause(false);

        var charactors = FindObjectsByType<MonoBehaviour>(
            FindObjectsInactive.Include,     
            FindObjectsSortMode.None         
        ).OfType<ICharactor>();
        foreach (var charactor in charactors)
        {
            _characterList.Add(charactor);
            charactor.SetupCharactor();
        }
    }

    void Update()
    {
        _characterList.RemoveAll(c => c == null);
        foreach(ICharactor charactor in _characterList)
        {
            charactor.UpdateCharactor();
        }

        switch( _currentGamePhase )
        {
            case GamePhase.Break:
                break;
            case GamePhase.Battle:
                _enemySpawner.EnemySpawn();
                _timer -= Time.deltaTime;
                if(_timer < 0)
                {
                    ChangePhase(GamePhase.Break);
                    _timer = 0;
                    _currentGamePhase = GamePhase.Break;
                    return;
                }
                int minutes = (int)(_timer / 60);
                int seconds = (int)(_timer % 60);
                _timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
                break;
        }
    }

    public void ChangePhase(GamePhase phase)
    {
        StartCoroutine(ChangePhaseWithFade(phase));
    }

    private IEnumerator ChangePhaseWithFade(GamePhase phase)
    {
        FadeOut();
        yield return new WaitForSeconds(_fadeDuration);

        switch (phase)
        {
            case GamePhase.Battle:
                _currentGamePhase = GamePhase.Battle;
                _timer = _phaseChangeTime;
                PhaseChange_Battle?.Invoke();
                _midText.text = "目標 : " + $"{_enemySpawner.CurrentPhaseIndex + 1}日目襲撃を生き延びる";
                RenderSettings.skybox = _nightMaterial;
                break;

            case GamePhase.Break:
                _currentGamePhase = GamePhase.Break;
                RenderSettings.skybox = _morningMaterial;
                _enemySpawner.CurrentPhaseIndex++;
                _midText.text = "目標 : " + "コアに触れて次の襲撃の準備をする";
                PhaseChange_Break?.Invoke();
                if(_enemySpawner.CurrentPhaseIndex == 5)
                {
                    _gameOverManager.StartGameClear();
                }
                break;
        }

        _ = ShowPhaseText();

        FadeIn();
    }

    public void BattlePhase()
    {
        ChangePhase(GamePhase.Battle);
    }

    public void AddIcharactorList(ICharactor charactor)
    {
        _characterList.Add(charactor);
    }

    private async Task ShowPhaseText()
    {
        switch(_currentGamePhase)
        {
            case GamePhase.Break:
                _pheseText.text = "脅威は去った";
                AudioManager.Instance.PlayBGM("Hiru");
                await UniTask.Delay(5000);
                _pheseText.text = "";
                break;
            case GamePhase.Battle:
                AudioManager.Instance.PlayBGM("Yoru");
                _pheseText.text = $"{_enemySpawner.CurrentPhaseIndex + 1}日目\n襲撃開始";
                await UniTask.Delay(5000);
                _pheseText.text = "";
                break;
        }
    }

    public void FadeOut()
    {
        // すでにフェード中なら止める
        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
        }

        _fadeCoroutine = StartCoroutine(Fade(0f, 1f));
    }

    /// <summary>
    /// フェードイン（明るくする）
    /// </summary>
    public void FadeIn()
    {
        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
        }

        _fadeCoroutine = StartCoroutine(Fade(1f, 0f));
    }

    /// <summary>
    /// 実際のフェード処理
    /// </summary>
    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;

        Color color = _fadeImage.color;
        color.a = startAlpha;
        _fadeImage.color = color;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / _fadeDuration;

            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            _fadeImage.color = color;

            yield return null;
        }

        // 念のため最終値を保証
        color.a = endAlpha;
        _fadeImage.color = color;
    }

    public void Mause(bool b)
    {
        if(b)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _playerController.Inventry();
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _playerController.Inventry();
        }
    }
}
