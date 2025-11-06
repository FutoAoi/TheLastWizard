using UnityEngine;
using UnityEngine.SceneManagement;

public class CoreController : MonoBehaviour ,IDamageable
{
    [Header("コアのステータス設定")]
    [SerializeField,Tooltip("コアの最大Hp")] private float _coreHp;

    public float HP => _coreHp;

    /// <summary>
    /// 破壊された時の処理
    /// </summary>
    public void Die()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// ヒット処理
    /// </summary>
    /// <param name="damage"></param>
    public void Hit(float damage)
    {
        _coreHp -= damage;
        Debug.Log($"コアが{damage}ダメージを受けている！！");
        if(_coreHp <= 0)
        {
            Die();
            _coreHp = 0;
        }
    }
}
