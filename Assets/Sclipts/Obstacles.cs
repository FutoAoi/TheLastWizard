using UnityEngine;
using UnityEngine.UI;

public class Obstacles : MonoBehaviour ,IDamageable
{
    [SerializeField] private float _maxHp;
    [SerializeField] private TeamType _teamType = TeamType.Player;
    [SerializeField] private Canvas _hpCanvas;
    [SerializeField] private Image _hpBar;
    [SerializeField] private bool _isIce;

    private GameObject _player;

    private float _currentHp;
    private float _timer;
    public float HP => _currentHp;

    public TeamType Team => _teamType;


    void Start()
    {
        _currentHp = _maxHp;
        _player = FindAnyObjectByType<PlayerController>().gameObject;
    }

    private void Update()
    {
        _hpCanvas.transform.LookAt(_player.transform);
        if(_isIce)
        {
            _timer += Time.deltaTime;
            if(_timer > 5)
            {
                Hit(1);
                _timer = 0;
            }
        }
    }

    public void Die()
    {
        if(_isIce)
        {
            Destroy(gameObject);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    public void Set()
    {
        gameObject.SetActive(true);
        _currentHp = _maxHp;
        HPUpdate();
    }

    public void Hit(float damage)
    {
        _currentHp -= damage;
        HPUpdate();
        if (_currentHp <= 0)
        {
            _currentHp = 0;
            Die();
        }
    }

    private void HPUpdate()
    {
        _hpBar.fillAmount = _currentHp / _maxHp;
    }
}
