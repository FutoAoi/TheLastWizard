using System;
using UnityEngine;

public class MagicBehavior : MonoBehaviour
{
    float _attackPower = 0;

    [SerializeField] private float _speed;
    [SerializeField] private float _destroyTimer;
    [SerializeField] private float _radius;
    [SerializeField] private GameObject _ice;
    
    private EffectObjectPool _pool;
    private MagicType _magicType;
    Transform _tf;

    private Action _onDisable;
    private float _elapsedTime;
    private void Start()
    {
        _tf = GetComponent<Transform>();
        _pool = EffectObjectPool.Instance;
    }

    public void Initialize(Action onDisable)
    {
        _onDisable = onDisable;
        _elapsedTime = 0;
    }

    private void Update()
    {
        _tf.Translate(Vector3.forward *  _speed * Time.deltaTime);

        RaycastHit hit;

        if(Physics.SphereCast(_tf.position, _radius, _tf.forward, out hit, _speed * Time.deltaTime))
        {
            GameObject hitTarget = hit.collider.gameObject;

            switch(_magicType)
            {
                case MagicType.Projectile:
                    IDamageable target = hitTarget.GetComponent<IDamageable>();
                    if (target != null)
                    {
                        if (target.Team == TeamType.Player) return;
                        EffectBehavior effect = _pool.GetEffect(EffectType.Hit);
                        effect.transform.position = _tf.position;
                        target.Hit(_attackPower);
                        DamagePopup.Create(_tf.position, _attackPower);
                    }
                    break;
                case MagicType.Area:
                    EffectBehavior thunder = _pool.GetEffect(EffectType.thunder);
                    thunder.transform.position = _tf.position;
                    break;
                case MagicType.Build:
                    Instantiate(_ice, _tf.position + (Vector3.up * 1.5f), Quaternion.identity);
                    break;
            }
            
            _onDisable?.Invoke();
            gameObject.SetActive(false);
        }

        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _destroyTimer)
        {
            _onDisable?.Invoke();
            gameObject.SetActive(false);
        }
    }

    public void AddStatus(float power, float range, float speed, MagicType type)
    {
        _attackPower = power;
        _destroyTimer = range;
        _speed = speed;
        _magicType = type;
    }

    private void OnDrawGizmos()
    {
        if (_tf == null) _tf = transform;

        Gizmos.color = Color.cyan;

        // åªç›à íuÇÃãÖÇï`âÊÅiìñÇΩÇËîªíËÇÃñ⁄à¿Åj
        Gizmos.DrawWireSphere(_tf.position, _radius);

    }
}