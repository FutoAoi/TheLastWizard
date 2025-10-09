using UnityEngine;

public class PlayerController : MonoBehaviour, ICharactor
{
    [SerializeField,Tooltip("最大体力")] float _maxHp;
    [SerializeField,Tooltip("最大マナ")] float _maxMp;
    [SerializeField,Tooltip("移動速度")] float _moveSpeed;

    private float _currentHp;
    private float _currentMp;
    private float _x, _y;
    private Vector3 _move;
    private Rigidbody _rb;

    //プロパティ
    public float HP => _currentHp;
    public float MoveSpeed => _moveSpeed;

    public void Setup()
    {
        _rb = GetComponent<Rigidbody>();
        _currentHp = _maxHp;
        _currentMp = _maxMp;
    }

    public void UpdateCharactor()
    {
        Move();
    }

    public void Die()
    {
        throw new System.NotImplementedException();
    }

    public void Hit(float damage)
    {
        throw new System.NotImplementedException();
    }

    private void Move()
    {
        _x = Input.GetAxis("Horizontal");
        _y = Input.GetAxis("Vertical");

        _move = transform.right * _x + transform.forward * _y;
        _move.Normalize();

        _rb.MovePosition(_rb.position + _move * _moveSpeed * Time.deltaTime);

    }
}
