using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerController : MonoBehaviour, ICharactor, IDamageable
{
    [Header("ステータス設定")]
    [SerializeField,Tooltip("最大体力")] float _maxHp;
    [SerializeField,Tooltip("最大マナ")] float _maxMp;
    [SerializeField,Tooltip("移動速度")] float _moveSpeed;

    [Header("カメラ設定")]
    [SerializeField] GameObject _mainCamera;
    [SerializeField] float _xSensitivity, _ySensitivity;
    [SerializeField] float _maxCameraAngle;

    [Header("コンポーネント設定")]
    [SerializeField] MagicShooter MagicShooter;

    private float _currentHp;
    private float _currentMp;
    private float _x, _z;
    private Vector3 _forward;
    private Vector3 _right;
    private Vector3 _moveDirection;
    private Rigidbody _rb;

    float _xRot, _yRot;
    float _clampYRot;
    Quaternion _playerRot;

    //プロパティ
    public float HP => _currentHp;
    public float MoveSpeed => _moveSpeed;
    public Quaternion PlayerRot => _playerRot;

    public void SetupCharactor()
    {
        _rb = GetComponent<Rigidbody>();
        _playerRot = transform.localRotation;
        _currentHp = _maxHp;
        _currentMp = _maxMp;
        MagicShooter.MagicUpdate();
    }

    public void UpdateCharactor()
    {
        Move();
        FPSCameraMove();
        MagicShooter.SetMagic();
        if (Input.GetMouseButton(0))
        {
            MagicShooter.MagicShoot();
        }
    }

    public void Die()
    {
        Debug.Log("死んだで");
    }

    public void Hit(float damage)
    {
        _currentHp -= damage;
        if( _currentHp < 0 )
        {
            Die();
        }
        Debug.Log($"{damage}受けた！！");
    }

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
}
