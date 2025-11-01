using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour,IDamageable,ICharactor
{
    [SerializeField] private float _hp;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _attckRange;
    [SerializeField] private float _chaseRange;

    private GameObject _defaultTarget;
    private GameObject _currentTarget;
    public float HP => _hp;

    void Awake()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.AddIcharactorList(this);
        }
        _defaultTarget = GameObject.Find("coa");
        _currentTarget = _defaultTarget;
    }

    public void SetupCharactor()
    {
        return;
    }

    public void UpdateCharactor()
    {
        if (_agent == null || !_agent.isActiveAndEnabled) return;

        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            float playerDistance = Vector3.Distance(transform.position, player.transform.position);

            float defaultDistance = _defaultTarget != null ? Vector3.Distance(transform.position, _defaultTarget.transform.position) : Mathf.Infinity;

            if (playerDistance < _chaseRange)
            {
                _currentTarget = player;
            }
            else
            {
                _currentTarget = _defaultTarget;
            }

        }
            float distance = (_currentTarget.transform.position - this.transform.position).magnitude;

        if(distance < _attckRange)
        {
            _agent.isStopped = true;
        }
        else
        {
            _agent.isStopped = false;
            _agent.SetDestination(_currentTarget.transform.position);
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }

    public void Hit(float damage)
    {
        _hp -= damage;
        if(_hp <= 0)
        {
            _hp = 0;
            Die();
        }
    }

}
