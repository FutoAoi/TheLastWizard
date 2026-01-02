using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour,IDamageable,ICharactor
{
    [Header("エネミーの基本ステータス")]
    [SerializeField,Tooltip("エネミーのHp")] private float _hp;
    [SerializeField,Tooltip("攻撃可能範囲")] private float _attckRange;
    [SerializeField,Tooltip("チェイス開始範囲")] private float _chaseRange;
    [SerializeField,Tooltip("攻撃判定の表示時間")] private float _showCollisionObjectTime;

    [Header("エネミーのコンポーネント設定")]
    [SerializeField,Tooltip("ナビメッシュ")] private NavMeshAgent _agent;
    [SerializeField,Tooltip("当たり判定のコライダー")] private GameObject _hitCheckerObject;

    private Animator _animator;
    private GameObject _defaultTarget;
    private GameObject _currentTarget;
    private GameObject _player;
    private bool _isWalking = false;
    private Action _onDisable;
    private bool _isDead = false;

    public float HP => _hp;

    void Awake()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.AddIcharactorList(this);
        }
        _animator = GetComponent<Animator>();
        _defaultTarget = FindAnyObjectByType<CoreController>().gameObject;
        _currentTarget = _defaultTarget;
        _player = GameObject.FindWithTag("Player");
    }

    void Start()
    {
        GameManager.instance.PhaseChange_Break += Die;
    }

    public void Initialized(Action onDisable)
    {
        _onDisable = onDisable;
        _isDead = false;
    }

    public void SetupCharactor()
    {
        return;
    }

    public void UpdateCharactor()
    {
        if (_agent == null || !_agent.isActiveAndEnabled) return;


        if (_player != null)
        {
            float playerDistance = Vector3.Distance(transform.position, _player.transform.position);

            float defaultDistance = _defaultTarget != null ? Vector3.Distance(transform.position, _defaultTarget.transform.position) : Mathf.Infinity;

            if (playerDistance < _chaseRange)
            {
                _currentTarget = _player;
            }
            else
            {
                _currentTarget = _defaultTarget;
            }

        }
            float distance = (_currentTarget.transform.position - this.transform.position).magnitude;

        if(distance < _attckRange)
        {
            _isWalking = false;
            _animator.SetTrigger("Attack");
        }
        else
        {
            _isWalking = true;
            _agent.SetDestination(_currentTarget.transform.position);
        }
        _agent.isStopped = !_isWalking;
        _animator.SetBool("isWalk", _isWalking);
    }
    
    public void Die()
    {
        if(_isDead) return;
        _isDead = true;
        _onDisable?.Invoke();
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

    public async UniTask Attack()
    {
        _hitCheckerObject.SetActive(true);

        await UniTask.Delay(TimeSpan.FromSeconds(_showCollisionObjectTime), cancellationToken: gameObject.GetCancellationTokenOnDestroy());

        _hitCheckerObject.SetActive(false);
    }
}
