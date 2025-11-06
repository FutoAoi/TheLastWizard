public interface IDamageable
{
    float HP { get; }
    void Hit(float damage);
    void Die();
}
