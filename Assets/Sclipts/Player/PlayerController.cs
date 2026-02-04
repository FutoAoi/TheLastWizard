using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, ICharactor, IDamageable
{

    [Header("ステータス設定")]
    [SerializeField, Tooltip("最大体力")] private float _maxHp;
    [SerializeField, Tooltip("最大スタミナ")] private float _maxSutamina;
    [SerializeField, Tooltip("スタミナ回復量")] private float _staminaRegeneration;
    [SerializeField, Tooltip("移動速度")] private float _moveSpeed;
    [SerializeField, Tooltip("ジャンプの強さ")] private float _jumpForce;
    [SerializeField, Tooltip("受ける最大の高さ")] private float _maxHeight;
    [SerializeField, Tooltip("陣営設定")] private TeamType _teamType = TeamType.Player;

    [Header("カメラ設定")]
    [SerializeField, Tooltip("FPSカメラ")] private GameObject _mainCamera;
    [SerializeField, Tooltip("カメラ感度")] public float _xSensitivity, _ySensitivity;
    [SerializeField, Tooltip("最大のカメラ傾き")] private float _maxCameraAngle;

    [Header("コンポーネント設定")]
    [SerializeField, Tooltip("マジックシューター")] PlayerAttackManager _attackManager;
    [SerializeField] private GameObject _tutoPanel;
    [SerializeField] private Slider _slider;
    [SerializeField] private GameOverManager _gameOverManager;

    [Header("行動範囲設定")]
    [SerializeField, Tooltip("行動範囲X軸")] private float _MaxPlayerAreaX;
    [SerializeField, Tooltip("行動範囲Z軸")] private float _MaxPlayerAreaZ;

    [Header("ダッシュ設定")]
    [SerializeField, Tooltip("ダッシュ時の速度倍率")]
    private float _dashSpeedMultiplier = 1.8f;

    [SerializeField, Tooltip("ダッシュ時のスタミナ消費量（毎秒）")]
    private float _dashStaminaCost = 20f;



    private float _currentHp;
    [SerializeField] private float _currentStamina;
    private float _x, _z;
    private Vector3 _forward;
    private Vector3 _right;
    private Vector3 _moveDirection;
    private Vector3 _currentPlayerPosition;
    private Rigidbody _rb;
    private Animator _animator;
    private Transform _tf;
    private float _startY;
    private float _currentHeight;
    private bool _isjumping = false;
    private bool _isOutOfStamina = false;
    private bool _isInventry = false;
    private float _xRot, _yRot;
    private float _clampYRot;
    private Quaternion _playerRot;
   [SerializeField] private UIManager _uiManager;

    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";

    private bool _isDashing = false;
    private bool _canDash = false;
    private bool _isDie = false;


    /// <summary>
    /// プロパティ
    /// </summary>
    public float HP => _currentHp;
    public float Stamina => _currentStamina;
    public float MaxHp => _maxHp;
    public float MaxSutamina => _maxSutamina;
    public Quaternion PlayerRot => _playerRot;
    public TeamType Team => _teamType;
    public PlayerAttackManager AttackManager => _attackManager;
    public bool IsInventry => _isInventry;

    /// <summary>
    /// 初期セットアップ
    /// </summary>
    public void SetupCharactor()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _tf = GetComponent<Transform>();
        _startY = _tf.position.y;
        _playerRot = transform.localRotation;
        _currentHp = _maxHp;
        _currentStamina = _maxSutamina;
        int i = 0;
        foreach(MagicSlotUI slot in _attackManager.MagicSlotUIs)
        {
            slot.SetIcon(_attackManager.Magics[i]);
            i++;
        }
    }

    /// <summary>
    /// 常時処理
    /// </summary>
    public void UpdateCharactor()
    {
        if(_isInventry) return;
        if(_isDie) return;
        Move();
        FPSCameraMove();
        Jump();
        Dash();
        SetSens();
        _attackManager.SetMagic();
        _attackManager.UpdateMagicUI();
        LimitArea();
        StaminaController();

        if (Input.GetMouseButtonDown(0))
        {
            _animator.Play("Attack", 0);
        }
        if(Input.GetMouseButtonDown(1))
        {
            _animator.Play("Melee", 0);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            _tutoPanel.SetActive(true);
            GameManager.instance.Mause(false);
        }
    }

    /// <summary>
    /// 死亡処理
    /// </summary>
    public void Die()
    {
        _gameOverManager.StartGameOver();
    }

    /// <summary>
    /// ヒット処理
    /// </summary>
    /// <param name="damage"></param>
    public void Hit(float damage)
    {
        if (_isDie) return;
        _currentHp -= damage;
        if( _currentHp < 0 )
        {
            Die();
            _currentHp = 0;
            _isDie = true;
        }
        Debug.Log($"{damage}受けた！！");
    }

    /// <summary>
    /// 行動メソット
    /// </summary>
    private void Move()
    {
        _x = Input.GetAxisRaw(_horizontal) * _moveSpeed;
        _z = Input.GetAxisRaw(_vertical) * _moveSpeed;

        if (Mathf.Abs(_x) < 0.1f) _x = 0f;
        if (Mathf.Abs(_z) < 0.1f) _z = 0f;

        _forward = _mainCamera.transform.forward; 
        _right = _mainCamera.transform.right;

        _forward.y = 0f;
        _right.y = 0f;
        _forward.Normalize();
        _right.Normalize();

        float speed = _moveSpeed;

        if (_isDashing)
        {
            speed *= _dashSpeedMultiplier;
        }

        _moveDirection = (_forward * _z + _right * _x).normalized * speed;

        _moveDirection.y = _rb.linearVelocity.y;

        _rb.linearVelocity = _moveDirection;
    }

    /// <summary>
    /// カメラメソット
    /// </summary>
    private void FPSCameraMove()
    {
        _xRot = Input.GetAxis("Mouse X") * _xSensitivity;
        _yRot = Input.GetAxis("Mouse Y") * _ySensitivity;

        _clampYRot -= _yRot;
        _clampYRot = Mathf.Clamp(_clampYRot, -_maxCameraAngle, _maxCameraAngle);

        _playerRot *= Quaternion.Euler(0f, _xRot, 0f);

        _mainCamera.transform.localRotation = Quaternion.Euler(_clampYRot, 0f, 0f);
        transform.localRotation = _playerRot;
    }

    /// <summary>
    /// ジャンプメソット
    /// </summary>
    public void Jump()
    {
        _currentHeight = _tf.position.y;
        if(Input.GetKey(KeyCode.Space) && _currentHeight < _startY + _maxHeight  && !_isOutOfStamina)
        {
            _isjumping = true;
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Acceleration);
            return;
        }
        else if(_currentHeight >= _startY + _maxHeight)
        {
            if(_rb.linearVelocity.y > 0f)
            {
                _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
            }
            return;
        }
        _isjumping = false;
    }

    private void StaminaController()
    {
        if(_isjumping)
        {
            _currentStamina -= _staminaRegeneration * Time.deltaTime;
            if(_currentStamina < 0f)
            {
                _currentStamina = 0f;
                StartCoroutine(OutOfStamina());
            }
        }
        else if(!_isjumping  && _currentStamina != _maxSutamina)
        {
            _currentStamina += _staminaRegeneration * Time.deltaTime / 2;
            if(_currentStamina >= _maxSutamina)
            {
                _currentStamina = _maxSutamina;
            }
        }
    }

    void LimitArea()
    {
        _currentPlayerPosition = _tf.transform.position;
        if(_currentPlayerPosition.x > _MaxPlayerAreaX)
        {
            _tf.position = new Vector3(_MaxPlayerAreaX, _currentPlayerPosition.y, _currentPlayerPosition.z);
        }
        if(_currentPlayerPosition.x < -_MaxPlayerAreaX)
        {
            _tf.position = new Vector3(-_MaxPlayerAreaX, _currentPlayerPosition.y, _currentPlayerPosition.z);
        }
        if(_currentPlayerPosition.z > _MaxPlayerAreaZ)
        {
            _tf.position = new Vector3(_currentPlayerPosition.x, _currentPlayerPosition.y, _MaxPlayerAreaZ);
        }
        if (_currentPlayerPosition.z < -_MaxPlayerAreaZ)
        {
            _tf.position = new Vector3(_currentPlayerPosition.x, _currentPlayerPosition.y, -_MaxPlayerAreaZ);
        }
    }

    private IEnumerator OutOfStamina()
    {
        _isOutOfStamina = true;
        yield return new WaitForSeconds(3);
        _isOutOfStamina = false;
    }

    public void Inventry()
    {
        _isInventry = !_isInventry;
        if(_rb != null)
        {
            _rb.linearVelocity = Vector3.zero;
        }
    }

    private void Dash()
    {
        if (!_canDash) return;
        if (Input.GetKey(KeyCode.LeftShift) && _currentStamina > 0f && _moveDirection.magnitude > 0.1f && !_isOutOfStamina)
        {
            _isDashing = true;

            _currentStamina -= _dashStaminaCost * Time.deltaTime;

            if (_currentStamina <= 0f)
            {
                _currentStamina = 0f;
                StartCoroutine(OutOfStamina());
                _isDashing = false;
            }
        }
        else
        {
            _isDashing = false;
        }
    }

    public void HpUp()
    {
        _maxHp += 10;
        _currentHp = _maxHp;
        _uiManager.MaxUpdate();
    }

    public void SutaminaUp()
    {
        _maxSutamina += 20;
        _uiManager.MaxUpdate();
    }

    public void CanDash()
    {
        _canDash = true;
    }

    private void SetSens()
    {
        if(_xSensitivity != _slider.value)
        {
            _xSensitivity = _slider.value;
            _ySensitivity = _slider.value;
        }
    }
}