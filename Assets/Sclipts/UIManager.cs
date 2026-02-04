using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("HPのUI設定")]
    [SerializeField] private Image _hpGauge;
    [SerializeField] private TMP_Text _maxHpText;
    [SerializeField] private TMP_Text _currentHpText;
    [Header("スタミナのUI設定")]
    [SerializeField] private Image _staminaGauge;
    [SerializeField] private TMP_Text _maxStaminaText;
    [SerializeField] private TMP_Text _currentStaminaText;
    [Header("CoreのUI設定")]
    [SerializeField] private Image _CoreGauge;
    [SerializeField] private TMP_Text _maxCoreText;
    [SerializeField] private TMP_Text _currentCoreText;
    [Header("強化画面")]
    [SerializeField] private GameObject _inventryPanel;
    [SerializeField] private Button _closeButton;

    private float _hp;
    private float _stamina;
    private float _maxHp;
    private float _maxStamina;
    private float _core;
    private float _maxCore;
    private EventSystem _eventSystem;

    private PlayerController _playerController;
    private PlayerAttackManager _attackManager;
    private CoreController _coreController;

    private void Start()
    {
        _playerController = FindAnyObjectByType<PlayerController>();
        _eventSystem = FindAnyObjectByType<EventSystem>();
        _coreController = FindAnyObjectByType<CoreController>();
        _attackManager = _playerController.AttackManager;
        _maxHp = _playerController.MaxHp;
        _maxStamina = _playerController.MaxSutamina;
        _maxCore = _coreController.HP;
        _maxHpText.text = "/" + _maxHp.ToString("F1");
        _maxStaminaText.text = "/" + _maxStamina.ToString("F1");
        _maxCoreText.text = "/" + _maxCore.ToString("F1");
        _closeButton.onClick.AddListener(ClosePanel);
    }

    private void Update()
    {
        if(_hp != _playerController.HP)
        {
            _hp = _playerController.HP;
            _currentHpText.text = _hp.ToString("F1");
            _hpGauge.fillAmount = _hp / _maxHp;
        }
        if(_stamina != _playerController.Stamina)
        {
            _stamina = _playerController.Stamina;
            _currentStaminaText.text = _stamina.ToString("F1");
            _staminaGauge.fillAmount = _stamina / _maxStamina;
        }
        if(_core != _coreController.CurrentHp)
        {
            _core = _coreController.CurrentHp;
            _currentCoreText.text = _core.ToString("F1");
            _CoreGauge.fillAmount= _core / _maxCore;
        }
    }

    public void MaxUpdate()
    {
        _maxHp = _playerController.MaxHp;
        _maxStamina = _playerController.MaxSutamina;
        _maxCore = _coreController.HP;
        _maxHpText.text = "/" + _maxHp.ToString("F1");
        _maxStaminaText.text = "/" + _maxStamina.ToString("F1");
        _maxCoreText.text = "/" + _maxCore.ToString("F1");
    }

    public void ShowPanel()
    {
        if(_playerController.IsInventry) return;
        _inventryPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _playerController.Inventry();
        _eventSystem.SetSelectedGameObject(null);
    }

    public void ClosePanel()
    {
        _inventryPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _playerController.Inventry();
    }
}
