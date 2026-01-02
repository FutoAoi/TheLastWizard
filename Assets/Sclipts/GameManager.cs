using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private List<ICharactor> _characterList = new List<ICharactor>();
    [SerializeField] private GamePhase _currentGamePhase = GamePhase.Break;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private TMP_Text _pheseText;
    public event Action PhaseChange_Break;
    public event Action PhaseChange_Battle;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

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
                break;
        }

        if(Input.GetKeyUp(KeyCode.E))
        {
            ChangePhase(GamePhase.Battle);
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            ChangePhase(GamePhase.Break);
        }
    }

    public void ChangePhase(GamePhase phase)
    {
        switch(phase)
        {
            case GamePhase.Battle:
                _currentGamePhase = GamePhase.Battle;
                PhaseChange_Battle?.Invoke();
                _ = ShowPhaseText();
                break;
            case GamePhase.Break:
                _currentGamePhase = GamePhase.Break;
                PhaseChange_Break?.Invoke();
                _ = ShowPhaseText();
                break;
        }
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
                _pheseText.text = "ã∫à–ÇÕãéÇ¡ÇΩ";
                await UniTask.Delay(3000);
                _pheseText.text = "";
                break;
            case GamePhase.Battle:
                _pheseText.text = "èPåÇäJénÅI";
                await UniTask.Delay(3000);
                _pheseText.text = "";
                break;
        }
    }
}
