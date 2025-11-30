using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image _hpGauge;
    [SerializeField] private Image _staminaGauge;

    private float _hp;
    private float _stamina;
    private float _maxHp;
    private float _maxStamina;

    private PlayerController _playerController;

    private void Start()
    {
        _playerController = FindAnyObjectByType<PlayerController>();
        _maxHp = _playerController.MaxHp;
        _maxStamina = _playerController.MaxSutamina;
    }

    private void Update()
    {
        if(_hp != _playerController.HP)
        {
            _hp = _playerController.HP;
            _hpGauge.fillAmount = _hp / _maxHp;
        }
        if(_stamina != _playerController.Stamina)
        {
            _stamina = _playerController.Stamina;
            _staminaGauge.fillAmount = _stamina / _maxStamina;
        }
    }
}
