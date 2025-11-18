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
    private bool _isWalking = false;
    private StateMachine<EnemyBase> _stateMachine;

    private readonly IState<EnemyBase> IdleState = new Idle();
    private readonly IState<EnemyBase> AttackState = new Attacks();
    private readonly IState<EnemyBase> CoreMoveState = new CoreMove();
    private readonly IState<EnemyBase> PurseState = new Pursue();
    public float HP => _hp;

    void Awake()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.AddIcharactorList(this);
        }
        _animator = GetComponent<Animator>();
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

    public async UniTask Attack()
    {
        _hitCheckerObject.SetActive(true);

        await UniTask.Delay(TimeSpan.FromSeconds(_showCollisionObjectTime), cancellationToken: gameObject.GetCancellationTokenOnDestroy());

        _hitCheckerObject.SetActive(false);
    }

    #region State
    public class Idle : IState<EnemyBase>
    {
        public void OnEnter(EnemyBase owner, IState<EnemyBase> prevState = null)
        {
            throw new NotImplementedException();
        }

        public void OnExit(EnemyBase owner, IState<EnemyBase> nextState = null)
        {
            throw new NotImplementedException();
        }

        public void OnUpdate(EnemyBase owner)
        {
            throw new NotImplementedException();
        }
    }

    public class Attacks : IState<EnemyBase>
    {
        public void OnEnter(EnemyBase owner, IState<EnemyBase> prevState = null)
        {
            throw new NotImplementedException();
        }

        public void OnExit(EnemyBase owner, IState<EnemyBase> nextState = null)
        {
            throw new NotImplementedException();
        }

        public void OnUpdate(EnemyBase owner)
        {
            throw new NotImplementedException();
        }
    }

    public class CoreMove : IState<EnemyBase>
    {
        public void OnEnter(EnemyBase owner, IState<EnemyBase> prevState = null)
        {
            throw new NotImplementedException();
        }

        public void OnExit(EnemyBase owner, IState<EnemyBase> nextState = null)
        {
            throw new NotImplementedException();
        }

        public void OnUpdate(EnemyBase owner)
        {
            throw new NotImplementedException();
        }
    }

    public class Pursue : IState<EnemyBase>
    {
        public void OnEnter(EnemyBase owner, IState<EnemyBase> prevState = null)
        {
            throw new NotImplementedException();
        }

        public void OnExit(EnemyBase owner, IState<EnemyBase> nextState = null)
        {
            throw new NotImplementedException();
        }

        public void OnUpdate(EnemyBase owner)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
}
