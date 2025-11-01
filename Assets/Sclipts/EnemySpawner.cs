using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject _enemy;
    [SerializeField] Transform[] _spawnPoint;
    [SerializeField] float _spawnTime;

    float _timer;

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
