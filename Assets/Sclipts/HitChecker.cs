using Cysharp.Threading.Tasks;
using UnityEngine;

public class HitChecker : MonoBehaviour
{
    private Transform _tf;
    [SerializeField] bool _isPlayer = false;

    private void Start()
    {
        _tf = GetComponent<Transform>();
    }

    public void OnTriggerEnter(Collider other)
    {
        IDamageable hittarget = other.gameObject.GetComponent<IDamageable>();
        if(hittarget != null)
        {
            EffectBehavior effect = EffectObjectPool.Instance.GetEffect(EffectType.Hit);
            effect.transform.position = _tf.position;
            hittarget.Hit(1);
            if(_isPlayer)
            {
                _ = HitStop();
            }
        }
    }

    private async UniTask HitStop()
    {
        Time.timeScale = 0.4f;
        await UniTask.Delay(80);
        Time.timeScale = 1.0f;
    }
}
