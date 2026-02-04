using UnityEngine;

public class TrainingDummy : MonoBehaviour ,IDamageable
{
    [SerializeField] private float _maxHp;
    [SerializeField] private TeamType _teamType = TeamType.Enemy;

    private Animator _animator;

    private float _currentHp;
    public float HP => _currentHp;

    public TeamType Team => _teamType;


    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Die()
    {
    }

    public void Hit(float damage)
    {
        _animator.Play("pushed");
        AudioManager.Instance.PlaySe("Hit");
    }
}
