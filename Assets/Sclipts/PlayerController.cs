using System.Collections;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerController : MonoBehaviour, ICharactor, IDamageable
{
    [Header("ステータス設定")]
    [SerializeField,Tooltip("最大体力")] private float _maxHp;
    [SerializeField,Tooltip("最大マナ")] private float _maxMp;
    [SerializeField,Tooltip("移動速度")] private float _moveSpeed;
    [SerializeField, Tooltip("ジャンプの強さ")] private float _jumpForce;
    [SerializeField, Tooltip("受ける最大の高さ")] private float _maxHeight;

    [Header("カメラ設定")]
    [SerializeField,Tooltip("FPSカメラ")] private GameObject _mainCamera;
    [SerializeField,Tooltip("カメラ感度")] public float _xSensitivity, _ySensitivity;
    [SerializeField,Tooltip("最大のカメラ傾き")] private float _maxCameraAngle;

    [Header("コンポーネント設定")]
    [SerializeField,Tooltip("マジックシューター")] MagicShooter MagicShooter;

    private float _currentHp;
    private float _currentMp;
    private float _x, _z;
    private Vector3 _forward;
    private Vector3 _right;
    private Vector3 _moveDirection;
    private Rigidbody _rb;
    private Animator _animator;
    private Transform _tf;
    private float _startY;

    private float _xRot, _yRot;
    private float _clampYRot;
    private Quaternion _playerRot;

    /// <summary>
    /// プロパティ
    /// </summary>
    public float HP => _currentHp;
    public float MoveSpeed => _moveSpeed;
    public Quaternion PlayerRot => _playerRot;

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
        _currentMp = _maxMp;
        MagicShooter.MagicUpdate();
    }

    /// <summary>
    /// 常時処理
    /// </summary>
    public void UpdateCharactor()
    {
        Move();
        FPSCameraMove();
        MagicShooter.SetMagic();
        Jump();
        if (Input.GetMouseButtonDown(0))
        {
            _animator.Play("Attack", 0);
        }
    }

    /// <summary>
    /// 死亡処理
    /// </summary>
    public void Die()
    {
        Debug.Log("死んだで");
    }

    /// <summary>
    /// ヒット処理
    /// </summary>
    /// <param name="damage"></param>
    public void Hit(float damage)
    {
        _currentHp -= damage;
        if( _currentHp < 0 )
        {
            Die();
        }
        Debug.Log($"{damage}受けた！！");
    }

    /// <summary>
    /// 行動メソット
    /// </summary>
    private void Move()
    {
        _x = Input.GetAxis("Horizontal") * _moveSpeed;
        _z = Input.GetAxis("Vertical") * _moveSpeed;

        if (Mathf.Abs(_x) < 0.1f) _x = 0f;
        if (Mathf.Abs(_z) < 0.1f) _z = 0f;

        _forward = _mainCamera.transform.forward; 
        _right = _mainCamera.transform.right;

        _forward.y = 0f;
        _right.y = 0f;
        _forward.Normalize();
        _right.Normalize();

        _moveDirection = (_forward * _z + _right * _x).normalized * _moveSpeed;

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

    public void Jump()
    {
        float currentHeight = _tf.position.y;
        if(Input.GetKey(KeyCode.Space) && currentHeight < _startY + _maxHeight )
        {
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Acceleration);
        }
        else if(currentHeight >= _startY + _maxHeight)
        {
            if(_rb.linearVelocity.y > 0f)
            {
                _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
            }
        }
    }
}
