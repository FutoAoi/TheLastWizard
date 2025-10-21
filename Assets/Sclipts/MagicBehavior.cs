using UnityEngine;

public class MagicBehavior : MonoBehaviour
{
    int _attackPower = 0;
    int _range = 0;

    [SerializeField] float _speed;
    [SerializeField] float _destroyTimer;
    Transform _tf;
    private void Start()
    {
        _tf = GetComponent<Transform>();
    }

    private void Update()
    {
        _tf.Translate(Vector3.forward *  _speed * Time.deltaTime);
        Destroy(gameObject, _range);
    }
    public void OnTriggerEnter(Collider other)
    {
        IDamageable target = other.GetComponent<IDamageable>();
        if(target != null)
        {
            target.Hit(_attackPower);
        }
        Destroy(this.gameObject);
    }

    public void AddStatus(int power, int range)
    {
        _attackPower = power;
        _range = range;
    }
}
