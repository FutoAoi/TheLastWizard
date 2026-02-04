public interface IDamageable
{

    float HP { get; }
    TeamType Team { get; }
    void Hit(float damage);
    void Die();
}

public enum TeamType
{
    Enemy,      // 敵
    Player,     // プレイヤー
    Core        // 守るコア
}
