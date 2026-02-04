using System.Collections;
using UnityEngine;

public class AreaDamage : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _radius;
    [SerializeField] private float _interval;
    [SerializeField] private float _duration;

    private ParticleSystem _system;

    private void Start()
    {
        _system = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        StartCoroutine(AreaAttackCoroutine());
    }

    IEnumerator AreaAttackCoroutine()
    {


        float timer = 0f;

        while (timer < _duration)
        {
            Collider[] hits = Physics.OverlapSphere(
                transform.position,
                _radius
            );

            foreach (Collider hit in hits)
            {
                IDamageable damageable = hit.GetComponent<IDamageable>();

                if (damageable != null && damageable.Team == TeamType.Enemy)
                {
                    damageable.Hit(_damage);
                    DamagePopup.Create(hit.transform.position + (Vector3.up * 3), _damage);
                }
            }

            yield return new WaitForSeconds(_interval);

            timer += _interval;
        }

        if (_system != null)
        {
            _system.Stop();
        }
    }

    /// <summary>
    /// Sceneƒrƒ…[‚ÅUŒ‚”ÍˆÍ‚ğ‰Â‹‰»
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
