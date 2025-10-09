public interface ICharactor
{
    float HP {  get; }
    float MoveSpeed {  get; }
    void UpdateCharactor();
    void Setup();
    void Hit(float damage);
    void Die();
}
