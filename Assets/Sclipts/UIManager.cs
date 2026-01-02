using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image _hpGauge;
    [SerializeField] private TMP_Text _maxHpText;
    [SerializeField] private TMP_Text _currentHpText;
    [SerializeField] private Image _staminaGauge;
    [SerializeField] private TMP_Text _maxStaminaText;
    [SerializeField] private TMP_Text _currentStaminaText;

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

    public void MaxUpdate(float hp, float stamina)
    {
        _maxHp = hp;
        _maxStamina = stamina;
    }
}
