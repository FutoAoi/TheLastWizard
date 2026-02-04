using UnityEngine;
using UnityEngine.SceneManagement;

public class CoreController : MonoBehaviour ,IDamageable ,IInteractable
{
    [Header("コアのステータス設定")]
    [SerializeField,Tooltip("コアの最大Hp")] private float _maxHp;
    [SerializeField,Tooltip("陣営設定")] private TeamType _teamType = TeamType.Core;
    [SerializeField] private GameOverManager _gameOverManager;


    private float _currentHp;
    public float HP => _maxHp;
    public float CurrentHp => _currentHp;

    public TeamType Team => _teamType;

    private void Start()
    {
        _currentHp = _maxHp;
    }

    /// <summary>
    /// 破壊された時の処理
    /// </summary>
    public void Die()
    {
        _gameOverManager.StartGameOver();
    }

    public string GetInteractText()
    {
        if (GameManager.instance.CurrentGamePhase == GamePhase.Battle)
        {
            return "戦闘中はコアに触れれない！";
        }
        return "[F]ソウルを使って強化する";
    }

    /// <summary>
    /// ヒット処理
    /// </summary>
    /// <param name="damage"></param>
    public void Hit(float damage)
    {
        _currentHp -= damage;
        Debug.Log($"コアが{damage}ダメージを受けている！！");
        if(_currentHp <= 0)
        {
            Die();
            _currentHp = 0;
        }
    }

    public void Interact(PlayerInteraction player)
    {
        if(GameManager.instance.CurrentGamePhase != GamePhase.Battle)
        {
            GameManager.instance.UIManager.ShowPanel();
        }
    }

    public void Heal()
    {
        _currentHp = _maxHp;
    }
}
