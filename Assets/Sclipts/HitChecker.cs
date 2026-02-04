using Cysharp.Threading.Tasks;
using UnityEngine;

public class HitChecker : MonoBehaviour
{
    private Transform _tf;
    [SerializeField] bool _isPlayer = false;
    [SerializeField] private float _attackPower;

    private void Start()
    {
        _tf = GetComponent<Transform>();
    }

    public void OnTriggerEnter(Collider other)
    {
        IDamageable hittarget = other.gameObject.GetComponent<IDamageable>();
        if(hittarget != null)
        {
            if(_isPlayer && hittarget.Team == TeamType.Player) return;
            EffectBehavior effect = EffectObjectPool.Instance.GetEffect(EffectType.Hit);
            effect.transform.position = _tf.position;
            hittarget.Hit(_attackPower);
            DamagePopup.Create(_tf.position,_attackPower);
            if (_isPlayer)
            {
                _ = HitStop();
            }
        }
    }

    public void AttackUp()
    {
        _attackPower += 1;
    }

    private async UniTask HitStop()
    {
        Time.timeScale = 0.4f;
        await UniTask.Delay(80);
        Time.timeScale = 1.0f;
    }
}
