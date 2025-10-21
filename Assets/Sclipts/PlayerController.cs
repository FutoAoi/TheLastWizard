using UnityEngine;

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
    private float _x, _y;
    private Vector3 _move;
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
        _x = Input.GetAxis("Horizontal");
        _y = Input.GetAxis("Vertical");

        if (Mathf.Abs(_x) < 0.1f) _x = 0f;
        if (Mathf.Abs(_y) < 0.1f) _y = 0f;

        _move = transform.right * _x + transform.forward * _y;
        _move.Normalize();

        _rb.MovePosition(_rb.position + _move * _moveSpeed * Time.deltaTime);
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
