using System;
using UnityEngine;

public class MagicBehavior : MonoBehaviour
{
    int _attackPower = 0;

    [SerializeField] float _speed;
    [SerializeField] float _destroyTimer;
    [SerializeField] float _radius;
    [SerializeField] GameObject _hitParticle;
    Transform _tf;

    private Action _onDisable;
    private float _elapsedTime;
    private void Start()
    {
        _tf = GetComponent<Transform>();
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

            IDamageable target = hitTarget.GetComponent<IDamageable>();
            if(target != null)
            {
                EffectBehavior effect = EffectObjectPool.Instance.GetEffect(EffectType.Hit);
                effect.transform.position = _tf.position;
                target.Hit(_attackPower);
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

    public void AddStatus(int power, int range, int speed)
    {
        _attackPower = power;
        _destroyTimer = range;
        _speed = speed;
    }

    private void OnDrawGizmos()
    {
        if (_tf == null) _tf = transform;

        Gizmos.color = Color.cyan;

        // åªç›à íuÇÃãÖÇï`âÊÅiìñÇΩÇËîªíËÇÃñ⁄à¿Åj
        Gizmos.DrawWireSphere(_tf.position, _radius);

    }
}