using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour,IDamageable,ICharactor
{
    [Header("エネミーの基本ステータス")]
    [SerializeField, Tooltip("敵の最大体力")] private float _maxHp;
    [SerializeField,Tooltip("攻撃可能範囲")] private float _attckRange;
    [SerializeField,Tooltip("チェイス開始範囲")] private float _chaseRange;
    [SerializeField,Tooltip("攻撃判定の表示時間")] private float _showCollisionObjectTime;
    [SerializeField] private float _damageableSearchRange = 5f;
    [SerializeField, Tooltip("陣営設定")] private TeamType _teamType = TeamType.Enemy;

    [Header("エネミーのコンポーネント設定")]
    [SerializeField,Tooltip("ナビメッシュ")] private NavMeshAgent _agent;
    [SerializeField,Tooltip("当たり判定のコライダー")] private GameObject _hitCheckerObject;
    [SerializeField] private Canvas _hpCanvas;
    [SerializeField] private Image _hpBar;

    private float _currentHp;
    private Animator _animator;
    private GameObject _defaultTarget;
    private GameObject _currentTarget;
    private GameObject _player;
    private bool _isWalking = false;
    private Action _onDisable;
    private bool _isDead = false;

    public float HP => _currentHp;

    public TeamType Team => _teamType;

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
        _currentHp = _maxHp;
        HPUpdate();
    }

    public void SetupCharactor()
    {
        return;
    }

    public void UpdateCharactor()
    {
        if (_agent == null || !_agent.isActiveAndEnabled) return;

        _hpCanvas.transform.LookAt(_player.transform);

        GameObject damageableTarget = FindNearestDamageable();

        if (damageableTarget != null)
        {
            _currentTarget = damageableTarget;
        }
        else if (_player != null)
        {
            float playerDistance = Vector3.Distance(transform.position, _player.transform.position);

            if (playerDistance < _chaseRange)
            {
                _currentTarget = _player;
            }
            else
            {
                _currentTarget = _defaultTarget;
            }
        }

 
        if (_currentTarget == null) return;

        float distance = Vector3.Distance(transform.position, _currentTarget.transform.position);

        if (distance < _attckRange)
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
        _currentHp -= damage;
        HPUpdate();
        _animator.Play("GetHit");
        AudioManager.Instance.PlaySe("Hit");
        _ = HitStop();
        if(_currentHp <= 0)
        {
            _currentHp = 0;
            SoulManager.Instance.GetSoul(100);
            Die();
        }
    }

    private void HPUpdate()
    {
        _hpBar.fillAmount = _currentHp/_maxHp;
    }

    GameObject FindNearestDamageable()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _damageableSearchRange);

        GameObject nearestTarget = null;
        float nearestDistance = Mathf.Infinity;

        foreach (Collider hit in hitColliders)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();

            // IDamageable を持っていなければ無視
            if (damageable == null) continue;

            // ★ 自分と同じ陣営（Enemy）は無視
            if (damageable.Team == TeamType.Enemy) continue;

            float distance = Vector3.Distance(transform.position, hit.transform.position);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestTarget = hit.gameObject;
            }
        }

        return nearestTarget;
    }

    public async UniTask Attack()
    {
        _hitCheckerObject.SetActive(true);

        await UniTask.Delay(TimeSpan.FromSeconds(_showCollisionObjectTime), cancellationToken: gameObject.GetCancellationTokenOnDestroy());

        _hitCheckerObject.SetActive(false);
    }

    public async UniTask HitStop()
    {
        _agent.speed = 0.5f;
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: gameObject.GetCancellationTokenOnDestroy());
        _agent.speed = 5;
    }
}
