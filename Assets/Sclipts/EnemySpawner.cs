using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("スポーンさせる敵")]
    [SerializeField] private GameObject _enemy;
    [Header("スポーンさせる場所")]
    [SerializeField] private Transform[] _spawnPoint;
    [Header("スポーン間隔")]
    [SerializeField] private float _spawnTime;

   　private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;
        if(_timer > _spawnTime)
        {
            int a = Random.Range(0, _spawnPoint.Length);
            Instantiate(_enemy, _spawnPoint[a].position, Quaternion.identity);
            _timer = 0;
        }
    }
}
