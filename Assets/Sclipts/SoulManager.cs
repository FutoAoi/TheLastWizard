using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class SoulManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _soulText;
    [SerializeField] private TMP_Text _inventryText;
    [SerializeField] private int _haveSoul;
    private float _firstScale;
    private float _scaleUpSize = 1.3f;
    private float _animationTime = 0.1f;
    public event Action<int> OnSoulChanged;
    public int Soul => _haveSoul;

    public static SoulManager Instance;

    public void Awake()
    {
        Instance = this;
        _firstScale = _soulText.fontSize;
        UpdateSoulText();
    }
    public void GetSoul(int soul)
    {
        _haveSoul += soul;
        OnSoulChanged?.Invoke(_haveSoul);
        UpdateSoulText();
        StopAllCoroutines();
        StartCoroutine(ScoreAnimation());
    }

    public bool UseSoul(int amount)
    {
        if (_haveSoul < amount) return false;

        _haveSoul -= amount;
        OnSoulChanged?.Invoke(_haveSoul);
        UpdateSoulText();
        return true;
    }

    public void UpdateSoulText()
    {
        _soulText.text = _haveSoul.ToString();
        _inventryText.text = _haveSoul.ToString();
    }

    IEnumerator ScoreAnimation()
    {
        float targetSize = _firstScale * _scaleUpSize;
        float timer = 0f;

        while (timer < _animationTime)
        {
            timer += Time.deltaTime;
            _soulText.fontSize = Mathf.Lerp(_firstScale, targetSize, timer / _animationTime);
            yield return null;
        }

        timer = 0f;

        while (timer < _animationTime)
        {
            timer += Time.deltaTime;
            _soulText.fontSize = Mathf.Lerp(targetSize, _firstScale, timer / _animationTime);
            yield return null;
        }

        _soulText.fontSize = _firstScale;
    }
}
